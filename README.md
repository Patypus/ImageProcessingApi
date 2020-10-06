# ImageProcessingApi

This is an service to serve up hosted images applying transformations based on the user's request, such as adding watermarks and updating the background colour.

## How to run

### Configuring the image source
The service serves up images from a configurable location which is defined by the `ImageSource` section of the app settings file. 
```
"ImageSource": {
    /* Update this to be the path to the folder containing images to serve up. */
    "Path": "D:\\images\\product_images",
    "DefaultImageFileType": "png"
  },
```
The `Path` defines the location to serve images from and the `DefaultImageFileType` the file extension to combine with the requested image name passed to the service.

### Configuring the cache
This service includes a layer to cache images to prevent reloading and re-transforming images to specifications that have previously been  requested. The current implementation uses Redis (although new implementations of the `Domain.Cache.Absractions` project using different systems could easily be substituted.). The default setting for the cache in master is to be disabled and the system will run without the cache. The application has been developed against [Memuari](https://www.memurai.com/) and uses the StackExchange.Redis library and is compatible with other Redis implementations.

To enable and configure the cache, see the `CacheConfiguration` section of the app settings file and set `CacheEnabled` to `true`. The endpoints for Redis are also configured in the same section and the defaults for Memuari are committed. 