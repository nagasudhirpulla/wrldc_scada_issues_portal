This is the application server code for SCADA issues tracking portal for wrldc

# Authorization rules

## Authorization rules for User management
* There will be only one super-admin user with credentials as per the app config
* Only super-admin and admin users can create / manage admin and guest users
* super-admin and admin users can change passwords / other details of any user

## Authorization rules for Issues
* Only admin users can manage issue info questions
* If a user is not in admin role, he can only see issues in which he is a creator / concerned agency
* Only admin / issue creator can edit an issue
* Only admin can delete an issue
* Only admin can close an issue
* Only admin can reopen an issue
* Only admin / issue creator / concerned agency can comment on the issue
* Only comment creator / admin can delete a comment