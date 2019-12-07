## 0.6.3 (2019-12-07)

### Release Information

Removed deadlock in the Robot.Dispose() method.  

### Features

*No new features in this release.*

### Known Issues

*None*.

### Bug Fixing

Removed deadlock in the Robot.Dispose() method.  

### Breaking Changes

*None*.


## 0.6.2 (2019-12-06)

### Release Information

Removed SynchronizationContext related code to improve performance and cleanup design.  

### Features

*No new features in this release.*

### Known Issues

*None*.

### Bug Fixing

Fixed FxCop analyzer issues

### Breaking Changes

Events now run on their own thread instead of thread that owns the Robot instance.


