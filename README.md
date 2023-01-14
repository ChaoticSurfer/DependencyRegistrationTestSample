<h1>Sample application demonstrating Test for verifying Dependencies requested from DI. </h1>
Test will build In Memory App Get ServiceCollection and try to create/Get all the Services requested in Controllers.

This is good candidate to be included in Automated Test suite for every CI/CD pipeline, before build is published to production.

Current way of scanning doesn't verify Resources from injected through Property-Attribute, or Middleware services injection, it only scans Controllers and their requested services.

If enough people are interested this will be made into Nuget Package and extended fully with all the ways DI can be used.
