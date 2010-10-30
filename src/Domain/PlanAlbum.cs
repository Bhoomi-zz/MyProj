using System;
using System.Collections.Generic;
using Ncqrs;
using Events;
using Ncqrs.Domain;
using CommonDTOs;
using Events.PlanAlbum;
using PhotoRemovedFromSite = Events.PlanAlbum.PhotoRemovedFromSite;

namespace Domain
{
    public class PlanAlbum : AggregateRootMappedByConvention
    {
        public Guid Id
        {
            get { return EventSourceId; }
            set { EventSourceId = value; }
        }

        private Guid _planAlbumId;
        private Guid _planDetailsId;
        private Dictionary<Guid, PlanCityAlbum> _cityAlbum;
        private List<Photo> _photos;

        public PlanAlbum()
        {}

        public PlanAlbum(Guid planDetailId, IEnumerable<PhotoDetailDTO> attachedPhotos)
        {
            var albumCreated = new PlanAlbumCreated();
            ApplyEvent(new PlanAlbumCreated() {PlanAlbumId = Id, PlanDetailsId = planDetailId });
            foreach (var photoDetailDto in attachedPhotos)
            {
                ApplyEvent(new PhotoAttachedToPlan(){PhotoId = photoDetailDto.PhotoId,  PhotoName = photoDetailDto.FilePath, Tags = photoDetailDto.Tags, Title = photoDetailDto.Title });
            }
        }

        private PlanCityAlbum GetCityAlbum(Guid planCityId)
        {
            if (_cityAlbum == null)
                _cityAlbum = new Dictionary<Guid, PlanCityAlbum>();

            PlanCityAlbum cityAlbum;
            if (_cityAlbum.ContainsKey(planCityId))
            {
                cityAlbum = _cityAlbum[planCityId];
            }
            else
            {
                cityAlbum = new PlanCityAlbum();
                _cityAlbum.Add(planCityId, cityAlbum);
            }
            if (cityAlbum.AttachedPhotos == null)
                cityAlbum.AttachedPhotos = new Dictionary<Guid, Photo>();
            return cityAlbum;
        }

        private PlanSiteAlbum GetSiteAlbum(PlanCityAlbum cityAlbum, Guid planSiteId)
        {
            if (cityAlbum.SiteAlbum == null)
                cityAlbum.SiteAlbum = new Dictionary<Guid, PlanSiteAlbum>();

            PlanSiteAlbum siteAlbum;
            if (cityAlbum.SiteAlbum.ContainsKey(planSiteId))
            {
                siteAlbum = cityAlbum.SiteAlbum[planSiteId];
            }
            else
            {
                siteAlbum = new PlanSiteAlbum();
                cityAlbum.SiteAlbum.Add(planSiteId, siteAlbum);
            }
            if (siteAlbum.AttachedPhotos == null)
                siteAlbum.AttachedPhotos = new Dictionary<Guid, Photo>();
            return siteAlbum;
        }

        public void AddOrRemovePhotosFromPlanAlbum(IEnumerable<PhotoDetailDTO> attachedPhotos, IEnumerable<PhotoDetailDTO> removedPhotos)
        {
            foreach (var photoDetailDto in attachedPhotos)
            {
                ApplyEvent(new PhotoAttachedToPlan() { PhotoId = photoDetailDto.PhotoId, PhotoName = photoDetailDto.FilePath, Tags = photoDetailDto.Tags, Title = photoDetailDto.Title });
            }
            foreach (var photoDetailDto in removedPhotos)
            {
                ApplyEvent(new PhotoRemovedFromPlanAlbum(){PhotoId = photoDetailDto.PhotoId, PlanAlbumId = Id });
            }
        }

        public void AddOrRemovePhotosFromPlanCity(IEnumerable<PhotoDetailDTO> attachedPhotos, IEnumerable<PhotoDetailDTO> removedPhotos, IEnumerable<PhotoDetailDTO> uploadedPhotos)
        {
            foreach (var photoDetailDto in attachedPhotos)
            {
                ApplyEvent(new PhotoAttachedToCity() { PhotoId = photoDetailDto.PhotoId, PhotoName = photoDetailDto.FilePath, PlanCityId = photoDetailDto.PlanCityId, Tags = photoDetailDto.Tags, Title = photoDetailDto.Title });
                //ApplyEvent(new PhotoRemovedFromPlanAlbum(){PhotoId = photoDetailDto.PhotoId });
            }
            foreach (var photoDetailDto in removedPhotos)
            {
                ApplyEvent(new PhotoRemovedFromCity() { PhotoId = photoDetailDto.PhotoId, PlanCityId = photoDetailDto.PlanCityId });
                //ApplyEvent(new PhotoAttachedToPlan(){ PhotoId = photoDetailDto.PhotoId, PhotoName = photoDetailDto.FilePath, Tags = photoDetailDto.Tags, Title = photoDetailDto.Title });
            }
            foreach (var photoDetailDto in uploadedPhotos)
            {
                ApplyEvent(new PhotoAttachedToCity() { PhotoId = photoDetailDto.PhotoId, PhotoName = photoDetailDto.FilePath, PlanCityId = photoDetailDto.PlanCityId, Tags = photoDetailDto.Tags, Title = photoDetailDto.Title });
            }  
        }

        public void AddOrRemovePhotosFromPlanSite(IEnumerable<PhotoDetailDTO> attachedPhotos, IEnumerable<PhotoDetailDTO> removedPhotos)
        {
            foreach (var photo in attachedPhotos)
            {
                ApplyEvent(new PhotoAttachedToSite() { PhotoId = photo.PhotoId, PhotoName = photo.FilePath, PlanSiteId = photo.PlanSiteId, PlanCityId = photo.PlanCityId, Tags = photo.Tags, Title = photo.Title });
            }
            foreach (var photo in removedPhotos)
            {
                ApplyEvent(new PhotoRemovedFromSite() {PhotoId = photo.PhotoId, PlanSiteId= photo.PlanSiteId,  PlanCityId= photo.PlanCityId } );
            }
        }

        public void OnPlanAlbumCreated(PlanAlbumCreated evnt)
        {
            _planAlbumId = evnt.PlanAlbumId;
            _planDetailsId = evnt.PlanDetailsId;
        }

        public void OnPhotoAttachedToPlan(PhotoAttachedToPlan evnt)
        {
            if (_photos == null)
                _photos = new List<Photo>();
            _photos.Add(new Photo() { PhotoName = evnt.PhotoName, PhotoId = evnt.PhotoId, Tags = evnt.Tags, Title = evnt.Title });
        }

        public void OnPhotoRemovedFromPlanAlbum(PhotoRemovedFromPlanAlbum evnt)
        {
            Photo photoDetail = _photos.Find(x => x.PhotoId == evnt.PhotoId);
            _photos.Remove(photoDetail);
        }
        
        public void OnPhotoAttachedToCity(PhotoAttachedToCity evnt)
        {
            var cityAlbum = GetCityAlbum(evnt.PlanCityId);
            
            Photo photoDetail = _photos.Find(x => x.PhotoId == evnt.PhotoId);
            if(photoDetail != null)
                _photos.Remove(photoDetail);

            if (!cityAlbum.AttachedPhotos.ContainsKey(evnt.PhotoId))
                cityAlbum.AttachedPhotos.Add(evnt.PhotoId,new Photo(){ PhotoId = evnt.PhotoId, PhotoName =evnt.PhotoName, Tags = evnt.Tags, Title = evnt.Title});
        }

        public void OnPhotoRemovedFromCity(PhotoRemovedFromCity evnt)
        {
            var cityAlbum = GetCityAlbum(evnt.PlanCityId);
            
            if (_photos == null)
                _photos = new List<Photo>();
            _photos.Add(new Photo() { PhotoName = "", PhotoId = evnt.PhotoId, Tags = "", Title = ""});

            if (cityAlbum.AttachedPhotos.ContainsKey(evnt.PhotoId))
                cityAlbum.AttachedPhotos.Remove(evnt.PhotoId);
        }

        public void OnPhotoAttachedToSite(PhotoAttachedToSite evnt)
        {
            PlanCityAlbum cityAlbum = GetCityAlbum(evnt.PlanCityId);
            if(cityAlbum.AttachedPhotos.ContainsKey(evnt.PhotoId))
                cityAlbum.AttachedPhotos.Remove(evnt.PhotoId);
            PlanSiteAlbum siteAlbum = GetSiteAlbum(cityAlbum, evnt.PlanSiteId);
            siteAlbum.PlanSiteId = evnt.PlanSiteId;
            if(!siteAlbum.AttachedPhotos.ContainsKey(evnt.PhotoId))
                siteAlbum.AttachedPhotos.Add(evnt.PhotoId, new Photo(){PhotoId = evnt.PhotoId, PhotoName =evnt.PhotoName, Tags= evnt.Tags, Title= evnt.Title });
        }
        public void OnPhotoRemovedFromSite(PhotoRemovedFromSite evnt)
        {
            PlanCityAlbum cityAlbum = GetCityAlbum(evnt.PlanCityId);
            PlanSiteAlbum siteAlbum = GetSiteAlbum(cityAlbum, evnt.PlanSiteId);
            siteAlbum.PlanSiteId = evnt.PlanSiteId;
            Photo photo = null;
            if (siteAlbum.AttachedPhotos.ContainsKey(evnt.PhotoId))
            {
                photo = siteAlbum.AttachedPhotos[evnt.PhotoId];
                siteAlbum.AttachedPhotos.Remove(evnt.PhotoId);
            }
            if (!cityAlbum.AttachedPhotos.ContainsKey(evnt.PhotoId))
                cityAlbum.AttachedPhotos.Add(evnt.PhotoId, photo);
        }
    }
}
