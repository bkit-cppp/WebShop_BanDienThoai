using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService()
        {
            var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
            var cloudinary = new Cloudinary(cloudinaryUrl);
            cloudinary.Api.Secure = true;
            _cloudinary = cloudinary;  
        }
        public async Task<IBussinessResult> DeleteImages(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
           
                return new BussinessResult(Const.FAIL_DELETE_CODE, "Không upload được hình ảnh");
            try
            {
                var uri = new Uri(imageUrl);
                var segments=uri.AbsolutePath.Split('/');
                var folder = segments[^2];
                var fileName = Path.GetFileNameWithoutExtension(segments[^1]);
                var publicId = $"{folder}/{fileName}";
                var deleteParams = new DeletionParams(publicId);
                var deleteResult =await _cloudinary.DestroyAsync(deleteParams);
                if (deleteResult.Result=="ok")
                    return new BussinessResult(Const.SUCCESS_DELETE_CODE, "Xóa ảnh thành công");
                return new BussinessResult(Const.FAIL_DELETE_CODE, "Xóa ảnh thất bại");
            }catch(Exception ex)
            {
                return new BussinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBussinessResult> UploadImages(IFormFile file, string folder, string publicId = null)
        {
            if(file==null || file.Length==0)
            {
                return new BussinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            try
            {
                var uploadParam = new ImageUploadParams()
                {
                    File=new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId=publicId,
                    Folder=folder
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParam);
                if (uploadResult?.StatusCode == HttpStatusCode.OK && uploadResult.SecureUrl!=null)
                
                    return new BussinessResult
                    {
                        Message=" UpLoad thành công",
                        Status=Const.SUCCESS_CREATE_CODE,
                        Data=uploadResult.SecureUrl.ToString()
                    };
                return new BussinessResult(Const.FAIL_CREATE_CODE, "Upload Hình ảnh không thành công, Kiểm tra lại cấu hình Cloudinary");
                                           
              // return new BussinessResult(Const.SUCCESS_CREATE_CODE, "")
            }catch(Exception ex)
            {
                return new BussinessResult(Const.FAIL_CREATE_CODE, $" Lỗi xảy ra:{ex.Message} ");
            }
        }
    }
}
