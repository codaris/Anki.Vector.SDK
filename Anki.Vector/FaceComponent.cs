// <copyright file="FaceComponent.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anki.Vector.Objects;
using Anki.Vector.Types;

namespace Anki.Vector
{
    /// <summary>
    /// Manage the state of the faces on the robot.
    /// </summary>
    public class FaceComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceComponent" /> class.
        /// </summary>
        /// <param name="robot">The robot.</param>
        internal FaceComponent(Robot robot) : base(robot)
        {
        }

        /// <summary>
        /// Gets the enrolled faces.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains an enumeration of <see cref="KnownFace"/> instances.</returns>
        public async Task<IEnumerable<KnownFace>> GetEnrolledFaces()
        {
            var items = new List<KnownFace>();
            var response = await Robot.RunMethod(client => client.RequestEnrolledNamesAsync(new ExternalInterface.RequestEnrolledNamesRequest())).ConfigureAwait(false);
            response.Status.EnsureSuccess();
            foreach (var face in response.Faces) items.Add(new KnownFace(face));
            return items.AsEnumerable();
        }

        /// <summary>
        /// Enrolls face into the robot (untested)
        /// </summary>
        /// <param name="name">The name of the person to enroll.</param>
        /// <param name="observedFaceId">The ID of a specific observed face to enroll (0 for next one we see)</param>
        /// <param name="saveToExistingFaceId">The ID of an existing face to merge final enrollment into (0 for none).</param>
        /// <param name="saveToRobot">Save the robot's NVStorage when done (NOTE: will (re)save everyone enrolled!)</param>
        /// <param name="sayName">Play say-name/celebration animations on success before completing</param>
        /// <param name="useMusic">Starts special music during say-name animations (will leave music playing)</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        private async Task<StatusCode> SetFaceToEnroll(string name, int observedFaceId, int saveToExistingFaceId, bool saveToRobot, bool sayName, bool useMusic)
        {
            var response = await Robot.RunMethod(r => r.SetFaceToEnrollAsync(new ExternalInterface.SetFaceToEnrollRequest()
            {
                Name = name,
                ObservedId = observedFaceId,
                SaveId = saveToExistingFaceId,
                SaveToRobot = saveToRobot,
                SayName = sayName,
                UseMusic = useMusic
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Enrolls face into the robot (untested)
        /// </summary>
        /// <param name="name">The name of the person to enroll.</param>
        /// <param name="saveToRobot">Save the robot's NVStorage when done (NOTE: will (re)save everyone enrolled!)</param>
        /// <param name="sayName">Play say-name/celebration animations on success before completing</param>
        /// <param name="useMusic">Starts special music during say-name animations (will leave music playing)</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> EnrollFace(string name, bool saveToRobot = true, bool sayName = true, bool useMusic = true)
        {
            return SetFaceToEnroll(name, 0, 0, saveToRobot, sayName, useMusic);
        }

        /// <summary>
        /// Enrolls face into the robot (untested)
        /// </summary>
        /// <param name="name">The name of the person to enroll.</param>
        /// <param name="observedFaceId">The ID of a specific observed face to enroll (0 for next one we see)</param>
        /// <param name="saveToRobot">Save the robot's NVStorage when done (NOTE: will (re)save everyone enrolled!)</param>
        /// <param name="sayName">Play say-name/celebration animations on success before completing</param>
        /// <param name="useMusic">Starts special music during say-name animations (will leave music playing)</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> EnrollFace(string name, int observedFaceId, bool saveToRobot = true, bool sayName = true, bool useMusic = true)
        {
            return SetFaceToEnroll(name, observedFaceId, 0, saveToRobot, sayName, useMusic);
        }

        /// <summary>
        /// Enrolls face into the robot (untested)
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="faceToEnroll">The specific observed face to enroll.</param>
        /// <param name="saveToRobot">Save the robot's NVStorage when done (NOTE: will (re)save everyone enrolled!)</param>
        /// <param name="sayName">Play say-name/celebration animations on success before completing</param>
        /// <param name="useMusic">Starts special music during say-name animations (will leave music playing)</param>
        /// <returns>
        /// A task that represents the asynchronous operation; the task result contains the result from the robot.
        /// </returns>
        /// <exception cref="ArgumentNullException">faceToEnroll</exception>
        public Task<StatusCode> EnrollFace(string name, Face faceToEnroll, bool saveToRobot = true, bool sayName = true, bool useMusic = true)
        {
            if (faceToEnroll == null) throw new ArgumentNullException(nameof(faceToEnroll));
            return EnrollFace(name, faceToEnroll.FaceId, saveToRobot, sayName, useMusic);
        }

        /// <summary>
        /// Updates an existing enrolled face
        /// </summary>
        /// <param name="faceToUpdate">An existing face to merge final enrollment into.</param>
        /// <param name="saveToRobot">Save the robot's NVStorage when done (NOTE: will (re)save everyone enrolled!)</param>
        /// <param name="sayName">Play say-name/celebration animations on success before completing</param>
        /// <param name="useMusic">Starts special music during say-name animations (will leave music playing)</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> UpdateEnrolledFace(KnownFace faceToUpdate, bool saveToRobot = true, bool sayName = true, bool useMusic = true)
        {
            if (faceToUpdate == null) throw new ArgumentNullException(nameof(faceToUpdate));
            return SetFaceToEnroll(faceToUpdate.Name, 0, faceToUpdate.FaceId, saveToRobot, sayName, useMusic);
        }

        /// <summary>
        /// Updates an existing enrolled face
        /// </summary>
        /// <param name="faceToUpdate">An existing face to merge final enrollment into.</param>
        /// <param name="faceToEnroll">The specific observed face to enroll.</param>
        /// <param name="saveToRobot">Save the robot's NVStorage when done (NOTE: will (re)save everyone enrolled!)</param>
        /// <param name="sayName">Play say-name/celebration animations on success before completing</param>
        /// <param name="useMusic">Starts special music during say-name animations (will leave music playing)</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> UpdateEnrolledFace(KnownFace faceToUpdate, Face faceToEnroll, bool saveToRobot = true, bool sayName = true, bool useMusic = true)
        {
            if (faceToUpdate == null) throw new ArgumentNullException(nameof(faceToUpdate));
            if (faceToEnroll == null) throw new ArgumentNullException(nameof(faceToEnroll));
            return SetFaceToEnroll(faceToUpdate.Name, faceToEnroll.FaceId, faceToUpdate.FaceId, saveToRobot, sayName, useMusic);
        }

        /// <summary>
        /// Updates an existing enrolled face
        /// </summary>
        /// <param name="faceToUpdate">An existing face to merge final enrollment into.</param>
        /// <param name="observedFaceId">The ID of a specific observed face to enroll (0 for next one we see)</param>
        /// <param name="saveToRobot">Save the robot's NVStorage when done (NOTE: will (re)save everyone enrolled!)</param>
        /// <param name="sayName">Play say-name/celebration animations on success before completing</param>
        /// <param name="useMusic">Starts special music during say-name animations (will leave music playing)</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public Task<StatusCode> UpdateEnrolledFace(KnownFace faceToUpdate, int observedFaceId, bool saveToRobot = true, bool sayName = true, bool useMusic = true)
        {
            if (faceToUpdate == null) throw new ArgumentNullException(nameof(faceToUpdate));
            return SetFaceToEnroll(faceToUpdate.Name, observedFaceId, faceToUpdate.FaceId, saveToRobot, sayName, useMusic);
        }

        /// <summary>
        /// Cancels the face enrollment.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> CancelFaceEnrollment()
        {
            var response = await Robot.RunMethod(r => r.CancelFaceEnrollmentAsync(new ExternalInterface.CancelFaceEnrollmentRequest())).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Updates the name of the enrolled face.
        /// </summary>
        /// <param name="faceId">The ID of the face to rename.</param>
        /// <param name="oldName">The old name of the face (must be correct, otherwise message is ignored).</param>
        /// <param name="newName">The new name for the face.</param>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> UpdateEnrolledFaceName(int faceId, string oldName, string newName)
        {
            var response = await Robot.RunMethod(client => client.UpdateEnrolledFaceByIDAsync(new ExternalInterface.UpdateEnrolledFaceByIDRequest()
            {
                FaceId = faceId,
                OldName = oldName,
                NewName = newName
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Updates the name of the enrolled face.
        /// </summary>
        /// <param name="face">The face to rename.</param>
        /// <param name="newName">The new name.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public Task<StatusCode> UpdateEnrolledFaceName(KnownFace face, string newName)
        {
            if (face == null) throw new ArgumentNullException(nameof(face));
            return UpdateEnrolledFaceName(face.FaceId, face.Name, newName);
        }

        /// <summary>
        /// Erases the enrollment (name) record for the face.
        /// </summary>
        /// <param name="faceId">The face identifier.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public async Task<StatusCode> EraseEnrolledFace(int faceId)
        {
            var response = await Robot.RunMethod(client => client.EraseEnrolledFaceByIDAsync(new ExternalInterface.EraseEnrolledFaceByIDRequest()
            {
                FaceId = faceId
            })).ConfigureAwait(false);
            return response.Status.Code.Convert();
        }

        /// <summary>
        /// Erases the enrollment (name) record for the face.
        /// </summary>
        /// <param name="face">The face.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains the result of the operation.</returns>
        public Task<StatusCode> EraseEnrolledFace(KnownFace face)
        {
            if (face == null) throw new ArgumentNullException(nameof(face));
            return EraseEnrolledFace(face.FaceId);
        }

        /// <summary>
        /// Erase the enrollment (name) records for all faces.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation; the task result contains the result from the robot.</returns>
        public async Task<StatusCode> EraseAllEnrolledFaces()
        {
            var response = await Robot.RunMethod(client => client.EraseAllEnrolledFacesAsync(new ExternalInterface.EraseAllEnrolledFacesRequest())).ConfigureAwait(false);
            return response.Status.Code.Convert();
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
