using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NotesRL:INotesRL
    {
        public string CLOUD_NAME = "dqydazifp";
        public string API_KEY = "346477855514539";
        public string API_SECRET = "p067hBd9T1nXWdvT2vL6-1mgsOwc";

        private readonly FundooContext fundooContext;

        public NotesRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public NotesEntity Create(NotesCreateModel notesCreateModel,long userid)
        {
            try
            {
                NotesEntity notes = new NotesEntity();
                notes.Title = notesCreateModel.Title;
                notes.Description = notesCreateModel.Description;
                notes.Image = notesCreateModel.Image;
                notes.Color = notesCreateModel.Color;
                notes.Archive = notesCreateModel.Archive;
                notes.Pin = notesCreateModel.Pin;
                notes.Trash = notesCreateModel.Trash;
                notes.Remainder = notesCreateModel.Remainder;
                notes.Createdat = DateTime.Now;
                notes.Modifiedat = notesCreateModel.Modifiedat;
                notes.UserId = userid;

                fundooContext.NotesTable.Add(notes);
                int result=fundooContext.SaveChanges();
                if (result > 0)
                {
                    return notes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<NotesEntity> GetAllNotes()
        {
            return fundooContext.NotesTable.ToList();
        }
        public IEnumerable<NotesEntity> GetNotesbyNoteid(long noteId)
        {
            return fundooContext.NotesTable.Where(x => x.NoteID == noteId).ToList();
        }

        public NotesEntity Update(NotesCreateModel notesCreateModel, long noteId)
        {
            try
            {
                NotesEntity result = fundooContext.NotesTable.Where(e => e.NoteID == noteId).FirstOrDefault();
                if (result != null)
                {
                    result.Title = notesCreateModel.Title;
                    result.Description = notesCreateModel.Description;
                    //result.Image = notesCreateModel.Image;
                    //result.Color = notesCreateModel.Color;
                    //result.Archive = notesCreateModel.Archive;
                    //result.Pin = notesCreateModel.Pin;
                    //result.Trash = notesCreateModel.Trash;
                    //result.Createdat = notesCreateModel.Createdat;
                    //result.Modifiedat = DateTime.Now;
                    fundooContext.NotesTable.Update(result);
                    fundooContext.SaveChanges();
                    return result;
                }
                else { return null; }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete(long noteId)
        {
            try
            {
                var result = fundooContext.NotesTable.FirstOrDefault(x => x.NoteID == noteId);
                fundooContext.Remove(result);
                int delete = fundooContext.SaveChanges();
                if (delete > 0)
                {
                    return true;
                }
                else { return false; } 
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity Archive(NotesIdModel notesIdModel)
        {
            try
            {
                NotesEntity result = fundooContext.NotesTable.FirstOrDefault(x => x.NoteID == notesIdModel.NoteID);
                if (result.Archive == true)
                {
                    result.Archive = false;
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    result.Archive = true;
                    fundooContext.SaveChanges();
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity Pin(long noteId)
        {
            try
            {
                NotesEntity result = fundooContext.NotesTable.FirstOrDefault(x => x.NoteID == noteId);
                if (result.Pin == true)
                {
                    result.Pin = false;
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    result.Pin = true;
                    fundooContext.SaveChanges();
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity Trash(long noteId)
        {
            try
            {
                NotesEntity result = fundooContext.NotesTable.FirstOrDefault(x => x.NoteID == noteId);
                if (result.Trash == true)
                {
                    result.Trash = false;
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    result.Trash = true;
                    fundooContext.SaveChanges();
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity Image(long noteid, IFormFile image)
        {
            try
            {
                var noteId = fundooContext.NotesTable.FirstOrDefault(e => e.NoteID == noteid);
                if (noteId != null)
                {
                    CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(CLOUD_NAME, API_KEY, API_SECRET);
                    Cloudinary cloud = new Cloudinary(account);
                    var imagePath = image.OpenReadStream();
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, imagePath)
                    };
                    var uploadresult = cloud.Upload(uploadParams);
                    noteId.Image = image.FileName;
                    fundooContext.NotesTable.Update(noteId);
                    int data = fundooContext.SaveChanges();
                    if (data > 0)
                    {
                        return noteId;
                    }
                }
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity Color(long noteid, string color)
        {
            try
            {
                NotesEntity note = this.fundooContext.NotesTable.FirstOrDefault(x => x.NoteID == noteid);
                if (note != null)
                {
                    note.Color = color;
                    this.fundooContext.SaveChanges();
                    return note;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
