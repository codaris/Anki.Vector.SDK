## 0.6.8 (2020-07-12)

### New Features

* Firmware 1.7 features
  * High resolution image capture
  * New events
    * Alexa Auth
    * Camera Settings Update
    * Check Update Status
    * Jdocs Changed
    * Observed motion
    * Unexpected movement
    * Erased enrolled face
    * Renamed enrolled face
  * Camera Configuration
  * Behavior cancellation
  * `SayText` pitch parameter
* Added `FirmwareVersion` property to `Robot` class
* Added `Capabilities` class to detecting firmware capabilities
* Added firmware update methods
* Added `CheckCloudConnection` method

### Bug fixes

* Video stream reliability greatly improved (no more blank video reboots required)
* Fixed connection failures for older versions of Windows
* Fixed phantom error when Vector's IP address changed
* Improved reliability of `WaitForAnimationCompletion` method

### Breaking Changes

* Camera images are now returned as `Image` instance in both the `CameraComponent` and `ImageReceivedEventArgs`.

---

## 0.6.7 (2020-03-29)

### Bug fixes

* Authentication fix

---

## 0.6.6 (2020-03-22)

### New Features

* AppIntent functions now release control automatically.
* Documentation improvements

### Known Issues

* Authentication issue

### Bug Fixes

* Lifetime stats bug fixes

### Breaking Changes

* AppIntent functions now release control automatically.
* Lifetime stats property type changes

---

## 0.6.5 (2020-02-16)

### New Features

* Ability to connect to Vector that is not on the local LAN (remote connections)
* Improved connection speed when connecting to known IP address
* FeatureStatus Event
* Submit AppIntent to Vector for processing
* REST API support
* Updated packages
* Improved error handling

### Bug Fixes

* Fixed cube light API bug
* Other minor fixes


---

## 0.6.4 (2019-12-09)

### Bug Fixes

* Fixed saving robot configuration when .anki_vector folder doesn't exist.


---

## 0.6.3 (2019-12-07)

### Bug Fixes

* Removed deadlock in the `Robot.Dispose()` method.  

---

## 0.6.2 (2019-12-06)

### Release Information

* Removed `SynchronizationContext` related code to improve performance and cleanup design.  

### Features

* *No new features in this release.*

### Bug Fixing

* Fixed FxCop analyzer issues

### Breaking Changes

* Events now run on their own thread instead of thread that owns the `Robot` instance.


