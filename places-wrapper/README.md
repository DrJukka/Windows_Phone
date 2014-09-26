Places Wrapper
==============

C# wrapper for Nokia Places REST API.

This is currently work on progress. This is basically made for WP8 the use case using
place_id with Maps launchers, anyway, it should also work with WP7 (if build for it), and
as it does not use launchers inside it, it could also be published separately and before
any launchers would be published.

The DLL in more-or-less feature complete, it should have nearly all things included in
Places Rest API implemented: ​http://api.maps.nokia.com/en/restplaces/overview.html,
things at least missing:

* making sure that the default values for index etc. do actually work
* adding W8 style await-version of the client 

The documentation is not yet started, so you could see the example, and maybe use the Rest
API as reference for functionality.

The example application is missing at least:

* UI designs for results showing
* Required Look& feel items (sponsored results differentiation, links to view the places
  etc.) docs. 
