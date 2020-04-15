using Grand.Core.Data;
using Grand.Core.Domain.Rewards;
using Grand.Services.Media;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.Rewards
{
    public partial class RewardService : IRewardService
    {
        #region Fields
        private readonly IRepository<Reward> _RewardRepository;
        private readonly IPictureService _pictureService;
        #endregion

        #region Ctor
        public RewardService(

        IRepository<Reward> RewardRepository,
        IPictureService pictureService

        )
        {
            this._RewardRepository = RewardRepository;
            this._pictureService = pictureService;
        }
        #endregion

        #region Method

        #region Add Reward Gift
        public virtual void AddRewardGift(Reward reward)
        {
            _RewardRepository.Insert(reward);
        }
        #endregion

        #region Get All Reward Gift
        public virtual List<Reward> GETAllRewardGiftInfo()
        {
            var query = from c in _RewardRepository.Table
                        where c.Delete == false
                        select c;
            return query.ToList();
        }

        public virtual List<Reward> GETAllValidRewardGiftInfo()
        {
            var query = from c in _RewardRepository.Table
                        where c.AvailableQuantity > 0 && c.Activate == true && c.Delete == false
                        select c;
            return query.ToList();
        }
        #endregion

        #region Get All Reward Gift No Exp
        public virtual List<Reward> GETAllRewardGiftInfoNoExp()
        {
            var query = from c in _RewardRepository.Table
                        where c.ExpiredTime > DateTime.UtcNow && c.Delete == false
                        select c;
            return query.ToList();
        }

        #endregion

        #region Get All Reward Gift Exp
        public virtual List<Reward> GETAllRewardGiftInfoExp()
        {
            var query = from c in _RewardRepository.Table
                        where c.ExpiredTime < DateTime.UtcNow && c.Delete == false
                        select c;
            return query.ToList();
        }

        #endregion

        #region Update Gift
        public virtual void UpdateGift(Reward gift)
        {
            _RewardRepository.Update(gift);
        }
        #endregion

        #region Get Lucky Draw Gift
        public virtual Reward GETGiftInfo(string id)
        {
            var query = from c in _RewardRepository.Table
                        where c.Id == id && c.Delete == false
                        select c;
            return query.ToList().FirstOrDefault();
        }
        #endregion

        public virtual void InsertRewardPicture(Reward product, string pictureId, int displayOrder, string overrideAltAttribute, string overrideTitleAttribute)
        {
            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                overrideAltAttribute,
                overrideTitleAttribute);

            //_productService.InsertRewardPicture(new ProductPicture
            //{
            //    PictureId = pictureId,
            //    ProductId = product.Id,
            //    DisplayOrder = displayOrder,
            //    AltAttribute = overrideAltAttribute,
            //    MimeType = picture.MimeType,
            //    SeoFilename = picture.SeoFilename,
            //    TitleAttribute = overrideTitleAttribute
            //});

            //_pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(product.Name));
        }
        #region Delete Lucky Draw Gift
        //public virtual void DeleteGift(LuckyDrawGiftManage gift)
        //{
        //    _LuckyDrawGiftManageRepository.Update(gift);
        //}
        //#endregion
        #endregion
        #endregion
    }
}
