using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Ncqrs.Eventing.ServiceModel.Bus;
using Events.PlanAlbum;

namespace ReadModel.Denormalizers
{
    public class PlanAlbumDenormalizer : IEventHandler<PlanAlbumCreated>, IEventHandler<PhotoAttachedToPlan>, IEventHandler<PhotoRemovedFromPlanAlbum>, IEventHandler<PhotoAttachedToCity>, IEventHandler<PhotoRemovedFromCity>, IEventHandler<PhotoAttachedToSite>, IEventHandler<PhotoRemovedFromSite>
    {

        public void Handle(PlanAlbumCreated evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var planAlbum = new PlanAlbum();
                planAlbum.planAlbumId = evnt.PlanAlbumId;
                planAlbum.AlbumNo = Series.AutoGenerateNumber(context, "PlanAlbum", "AlbumNo");
                planAlbum.planDetailsId = evnt.PlanDetailsId;
                context.PlanAlbums.AddObject(planAlbum);
                context.SaveChanges();
            }
        }

        public void Handle(PhotoAttachedToPlan evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var sitePhoto = context.PlanAlbumSitePhotos.SingleOrDefault(x => x.PlanAlbumSitePhotosId == evnt.PhotoId);
                bool NewlyAdded = false;
                if (sitePhoto == null)
                {
                    NewlyAdded = true;
                    sitePhoto = new PlanAlbumSitePhoto();
                    sitePhoto.PlanAlbumSitePhotosId = evnt.PhotoId;
                    sitePhoto.planAlbumId = evnt.EventSourceId;
                }
                sitePhoto.PhotoName = evnt.PhotoName;
                sitePhoto.Title = evnt.Title;
                sitePhoto.Tags = evnt.Tags;
                sitePhoto.PlanCityId = null;
                sitePhoto.PlanSiteId = null;
                sitePhoto.planAlbumId = evnt.EventSourceId;
                
                if(NewlyAdded)
                    context.PlanAlbumSitePhotos.AddObject(sitePhoto);
                context.SaveChanges();
            }
        }

        public void Handle(PhotoRemovedFromPlanAlbum evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var sitePhoto = context.PlanAlbumSitePhotos.SingleOrDefault(x => x.PlanAlbumSitePhotosId == evnt.PhotoId);
                //sitePhoto.bIsAssigned = false;
                if (sitePhoto != null)
                {
                    context.PlanAlbumSitePhotos.DeleteObject(sitePhoto);
                    context.SaveChanges();
                }
            }
        }

        public void Handle(PhotoAttachedToCity evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var photo = context.PlanAlbumSitePhotos.SingleOrDefault(x => x.PlanAlbumSitePhotosId == evnt.PhotoId);
                if (photo == null)
                {
                    photo = new PlanAlbumSitePhoto();
                    photo.planAlbumId = evnt.EventSourceId;
                    photo.PlanAlbumSitePhotosId = evnt.PhotoId;
                    photo.PlanCityId = evnt.PlanCityId;
                    photo.PlanSiteId = null;
                    photo.Title = evnt.Title;
                    photo.Tags = evnt.Tags;
                    photo.PhotoName = evnt.Title;
                    context.PlanAlbumSitePhotos.AddObject(photo);
                }
                else
                {
                    photo.PlanCityId = evnt.PlanCityId;
                    photo.PlanSiteId = null;
                    photo.Title = evnt.Title;
                    photo.Tags = evnt.Tags;
                    photo.PhotoName = evnt.Title;
                }
                context.SaveChanges();
            }
        }

        public void Handle(PhotoRemovedFromCity evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var sitePhoto = context.PlanAlbumSitePhotos.Single(x => x.PlanAlbumSitePhotosId == evnt.PhotoId);
                sitePhoto.PlanCityId = null;
                context.SaveChanges();
            }
        }

        public void Handle(PhotoAttachedToSite evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var photo = context.PlanAlbumSitePhotos.SingleOrDefault(x => x.PlanAlbumSitePhotosId == evnt.PhotoId);
                photo.PlanCityId = evnt.PlanCityId;
                photo.PlanSiteId = evnt.PlanSiteId;
                photo.Title = evnt.Title;
                photo.Tags = evnt.Tags;
                photo.PhotoName = evnt.Title;
                context.SaveChanges();
            }
        }

        public void Handle(PhotoRemovedFromSite evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var sitePhoto = context.PlanAlbumSitePhotos.Single(x => x.PlanAlbumSitePhotosId == evnt.PhotoId);
                sitePhoto.PlanCityId = evnt.PlanCityId;
                sitePhoto.PlanSiteId = null;
                context.SaveChanges();
            }
        }
    }
}
