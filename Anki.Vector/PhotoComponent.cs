// <copyright file="PhotoComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anki.Vector.Exceptions;
using Anki.Vector.Types;

namespace Anki.Vector
{
    /// <summary>
    /// Access the photos on Vector.
    /// </summary>
    public class PhotoComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal PhotoComponent(Robot robot) : base(robot)
        {
        }

        /// <summary>
        /// Request the photo information from the robot.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<IEnumerable<PhotoInfo>> GetPhotoInfo()
        {
            var items = new List<PhotoInfo>();
            var response = await Robot.RunMethod(client => client.PhotosInfoAsync(new ExternalInterface.PhotosInfoRequest())).ConfigureAwait(false);
            response.Status.EnsureSuccess();
            foreach (var photoInfo in response.PhotoInfos) items.Add(new PhotoInfo(photoInfo));
            return items.AsEnumerable();
        }

        /// <summary>
        /// Download a full-resolution photo from the robot's storage.
        /// </summary>
        /// <param name="photoId">The photo identifier.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        /// <exception cref="VectorRequestException">Unable to retrieve photo from Vector</exception>
        public async Task<byte[]> GetPhoto(uint photoId)
        {
            var response = await Robot.RunMethod(client => client.PhotoAsync(new ExternalInterface.PhotoRequest()
            {
                PhotoId = photoId
            })).ConfigureAwait(false); ;
            if (response.Success) return response.Image.ToByteArray();
            throw new VectorRequestException(response.Status.Code.Convert(), "Unable to retrieve photo from Vector");
        }

        /// <summary>
        /// Download a full-resolution photo from the robot's storage.
        /// </summary>
        /// <param name="photoInfo">The photo information.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public Task<byte[]> GetPhoto(PhotoInfo photoInfo)
        {
            if (photoInfo == null) throw new ArgumentNullException(nameof(photoInfo));
            return GetPhoto(photoInfo.PhotoId);
        }

        /// <summary>
        /// Download a thumbnail of a given photo from the robot's storage.
        /// <para>You may use this function to pull all of the images off the robot in a smaller format, and then determine which one to download as full resolution.</para>
        /// </summary>
        /// <param name="photoId">The photo identifier.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        /// <exception cref="VectorRequestException">Unable to retrieve photo from Vector</exception>
        public async Task<byte[]> GetThumbnail(uint photoId)
        {
            var response = await Robot.RunMethod(client => client.ThumbnailAsync(new ExternalInterface.ThumbnailRequest()
            {
                PhotoId = photoId
            })).ConfigureAwait(false); ;
            if (response.Success) return response.Image.ToByteArray();
            throw new VectorRequestException(response.Status.Code.Convert(), "Unable to retrieve photo from Vector");
        }

        /// <summary>
        /// Download a thumbnail of a given photo from the robot's storage.
        /// <para>You may use this function to pull all of the images off the robot in a smaller format, and then determine which one to download as full resolution.</para>
        /// </summary>
        /// <param name="photoInfo">The photo information.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        /// <exception cref="VectorRequestException">Unable to retrieve photo from Vector</exception>
        public Task<byte[]> GetThumbnail(PhotoInfo photoInfo)
        {
            if (photoInfo == null) throw new ArgumentNullException(nameof(photoInfo));
            return GetThumbnail(photoInfo.PhotoId);
        }

        /// <summary>
        /// Deletes the photo from the robot.
        /// </summary>
        /// <param name="photoId">The photo identifier.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> DeletePhoto(uint photoId)
        {
            var response = await Robot.RunMethod(client => client.DeletePhotoAsync(new ExternalInterface.DeletePhotoRequest()
            {
                PhotoId = photoId
            })).ConfigureAwait(false); ;
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Deletes the photo from the robot.
        /// </summary>
        /// <param name="photoInfo">The photo information.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public Task<StatusCode> DeletePhoto(PhotoInfo photoInfo)
        {
            if (photoInfo == null) throw new ArgumentNullException(nameof(photoInfo));
            return DeletePhoto(photoInfo.PhotoId);
        }

        /// <summary>
        /// Called when disconnecting
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal override Task Teardown()
        {
            return Task.CompletedTask;
        }
    }
}
