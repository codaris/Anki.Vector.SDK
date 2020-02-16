## 0.6.5 (2020-02-16)

### New Features

* Ability to connect to Vector that is not on the local LAN (remote connections)
* Improved connection speed when connecting to known IP address
* FeatureStatus Event
* Submit AppIntent to Vector for processing
* REST API support
* Updated packages
* Improved error handling

### Known Issues

* *None*.

### Bug Fixes

* Fixed cube light API bug
* Other minor fixes

### Breaking Changes

* *None*.

## 0.6.4 (2019-12-09)

### New Features

* *No new features in this release.*

### Known Issues

* *None*.

### Bug Fixes

* Fixed saving robot configuration when .anki_vector folder doesn't exist.

### Breaking Changes

* *None*.


## 0.6.3 (2019-12-07)

### New Features

* *No new features in this release.*

### Known Issues

* *None*.

### Bug Fixes

* Removed deadlock in the Robot.Dispose() method.  

### Breaking Changes

* *None*.


## 0.6.2 (2019-12-06)

### Release Information

* Removed SynchronizationContext related code to improve performance and cleanup design.  

### Features

* *No new features in this release.*

### Known Issues

* *None*.

### Bug Fixing

* Fixed FxCop analyzer issues

### Breaking Changes

* Events now run on their own thread instead of thread that owns the Robot instance.


