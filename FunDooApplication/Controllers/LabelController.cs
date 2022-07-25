using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunDooApplication.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public LabelController(ILabelBL labelBL,IMemoryCache memoryCache,IDistributedCache distributedCache)
        {
            this.labelBL = labelBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        [HttpPost("Add")]
        public IActionResult Create(LabelNameIdModel labelModel)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.First(x => x.Type == "userId").Value);
                var result = labelBL.Create(labelModel.noteId,userId, labelModel.LabelName) ;
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Labels Created Successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to Create" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpDelete("Remove")]
        public IActionResult Delete(LabelNameModel labelNameModel)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                if (labelBL.Delete(userID, labelNameModel.LabelName))
                {
                    return this.Ok(new { success = true, message = "Label deleted successfully" });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "User access denied" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPut("Rename")]
        public IActionResult UpdateName(LabelRenameModel label)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                var result = labelBL.UpdateName(userID, label.LabelName,label.NewLabelName) ;
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "Label renamed successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Unable to rename" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("AllLabel")]
        public IEnumerable<LabelEntity> GetAllLabel()
        {
            try
            {
                return labelBL.GetAllLabel();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetbyID")]
        public IEnumerable<LabelEntity> GetLabelByID(long noteid)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);
                return labelBL.GetLabelByID(noteid, userID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllLabelUsingRedisCache()
        {
            var cacheKey = "LabelList";
            string serializedLabelList;
            var LabelList = new List<LabelEntity>();
            var redisLabelList = await distributedCache.GetAsync(cacheKey);
            if (redisLabelList != null)
            {
                serializedLabelList = Encoding.UTF8.GetString(redisLabelList);
                LabelList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
            }
            else
            {
                LabelList = (List<LabelEntity>)labelBL.GetAllLabel();
                serializedLabelList = JsonConvert.SerializeObject(LabelList);
                redisLabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLabelList, options);
            }
            return Ok(LabelList);
        }
    }
}
