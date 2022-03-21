# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.3.1](https://github.com/unity-game-framework/ugf-webrequests/releases/tag/1.3.1) - 2022-03-21  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-webrequests/milestone/7?closed=1)  
    

### Fixed

- Fix HttpWebRequestSender content headers setup ([#15](https://github.com/unity-game-framework/ugf-webrequests/issues/15))  
    - FIx `HttpWebRequestSender` class request headers setup for body content.
- Fix HttpWebRequestSender max buffer size setup ([#14](https://github.com/unity-game-framework/ugf-webrequests/issues/14))  
    - Fix `HttpWebRequestSenderDescription.MaxResponseContentBufferSize` property to return `int` value.
    - Change `HttpWebRequestSenderDescription` class to have options to specific override for `Timeout` and `MaxBuffer` properties.

## [1.3.0](https://github.com/unity-game-framework/ugf-webrequests/releases/tag/1.3.0) - 2022-03-18  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-webrequests/milestone/6?closed=1)  
    

### Added

- Add unity web request send error message into response ([#12](https://github.com/unity-game-framework/ugf-webrequests/issues/12))  
    - Add error message in response as data when no data provided and error has occurred.

## [1.2.0](https://github.com/unity-game-framework/ugf-webrequests/releases/tag/1.2.0) - 2022-03-02  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-webrequests/milestone/5?closed=1)  
    

### Added

- Add sender and receiver builder assets ([#10](https://github.com/unity-game-framework/ugf-webrequests/issues/10))  
    - Update dependencies: `com.ugf.initialize` to `2.8.0` version.
    - Add `HttpWebRequestReceiverAsset` class as asset to build receiver for _Http_ solution.
    - Add `HttpWebRequestSenderAsset` class as asset to build sender for _Http_ solution.
    - Add `UnityWebRequestSenderAsset` class as asset to builder sender for _Unity_ solution.

## [1.1.3](https://github.com/unity-game-framework/ugf-webrequests/releases/tag/1.1.3) - 2022-01-09  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-webrequests/milestone/4?closed=1)  
    

### Fixed

- Fix to do not setup empty byte array as data ([#8](https://github.com/unity-game-framework/ugf-webrequests/issues/8))  
    - Change `HttpWebRequestReceiver`, `HttpWebRequestSender` and `UnityWebRequestSender` classes to skip data when it is an empty array.

## [1.1.2](https://github.com/unity-game-framework/ugf-webrequests/releases/tag/1.1.2) - 2021-12-15  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-webrequests/milestone/3?closed=1)  
    

### Fixed

- Fix UnityWebRequestSender set null data from download handler ([#7](https://github.com/unity-game-framework/ugf-webrequests/pull/7))  
    - Fix  `UnityWebRequestSender` class to check data from download handler when setup data for response.

## [1.1.1](https://github.com/unity-game-framework/ugf-webrequests/releases/tag/1.1.1) - 2021-11-29  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-webrequests/milestone/2?closed=1)  
    

### Fixed

- Fix publish registry ([#5](https://github.com/unity-game-framework/ugf-webrequests/pull/5))  
    - Fix package publish registry.

## [1.1.0](https://github.com/unity-game-framework/ugf-webrequests/releases/tag/1.1.0) - 2021-11-29  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-webrequests/milestone/1?closed=1)  
    

### Added

- Add web message get data with generic argument ([#3](https://github.com/unity-game-framework/ugf-webrequests/pull/3))  
    - Update package _Unity_ version to `2021.2`.
    - Update dependencies: `com.ugf.initialize` to `2.7.0` and `com.ugf.logs` to `5.2.2`.
    - Update package publish registry.
    - Add `IWebMessage.TryGetData<T>()` and `GetData<T>()` methods.

## [1.0.0](https://github.com/unity-game-framework/ugf-webrequests/releases/tag/1.0.0) - 2021-01-05  

### Release Notes

- No release notes.


