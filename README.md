# My-Car-Log-Windows-Phone
This is a historical archive of My Car Log that was released from 2013 through 2015 for Windows Phone 8 and 8.1. I created this as a learning project on how to create mobile applications for Windows Phone as well as something that I needed. Unfortunately when Windows Phone 8 was deprecated and the source was completely incompatible with Windows Phone 10, I was left with the choice of rewriting a lot of the program from scratch or dropping it. At the time I was fairly busy and felt disenfranchised by the forced incompatibility, as well as the fear that I would finish my update and Microsoft would just change everything again, so I dropped it.

Now I use Android, so maybe someday I'll release a version for that, but until then...

# Features
My Car Log could:
* Keep track of multiple vehicles and their loans
* Keep track of Fuel and Service events
* Displayed several statistics and, if I recall, could even graph some of the key values out.
* Allowed you to save your data to OneDrive so that if you switched phones you didn't lose all your information.

# Building
As far as building this, I'm not currently spending a lot of time on it, but you would need the Windows Phone 8 development kit to even attempt it.

# Release Notes
Here are the release final release notes associated with it:

My Car Log allows you to track basic information about your car, when you refuel, and keep a history of your service events. In addition to keeping track of when you refuel your car, it will display useful information about those transactions (e.g. Miles per Gallon) and provides an overall summary of your vehicle's performance including total costs for several different time periods.

My Car Log supports multiple vehicles and provides an individual summary of events for each car. Distances can be tracked in either Miles or Kilometers and quantity of fuel can be tracked in either gallons or liters.

1.0.0.0 - Initial Release
 * Advertisement now hides when there is no add to display
 * Fixed issue with removing characters that are used for the save file for string values

1.1.0.0 - 
 * Removed ID_CAP_MEDIALIB_AUDIO, ID_CAP_MEDIALIB_PLAYBACK, ID_CAP_SENSORS as they are not used
 * Added Refuel Averages for Price and Distance Traveled between Refueling
 * Web and Email now open web browser and email client respectively
 * Events now sort by Date and Odometer instead of just Odometer

1.2.0.0 -
 * Settings page created. Current settings are for distance (miles or kilometers) and quantity (gallons or liters)
 * Pages updated to support settings page selections
 * Added support for keeping track of Insurance Information
 * Added support for keeping track of Bank Loan Information
 * Added Total Costs based on Month to Date (MTD), Year to Date (YTD), 1 Month (Previous Complete Month), 3 Month (Previous 3 Complete Months), 6 Months (Previous 6 Complete Months), and 12 Months (Previous 12 Complete Months)

1.2.1.0 -
 * Fixed the Scheduled Maintenance Light Theme image so that it now appears
 * Fixed bug causing the car page to crash because of service events
 * Changed all NaN's to 0 or 1 as this was causing hanging in advanced calculations

1.3.0.0 -
 * For new fuel events, the average distance between refueling is added to the last odometer to give an estimate of the next fill up. The entry no longer clears out because of this.
 * Reminders have been added. Provides an approximate based on the type of reminder and the factors that build it (e.g. a reminder for 3,000 miles will approximate your daily usage and find out when 3,000 miles will occur from the last service odometer)
 * Reminders can also be turned into Service events. If the Reminder is a reoccurring one, it will create the next instance of itself when the new service event is saved.
 * Compact View has been implemented. You can now see the main screen with entries in a compact view format.
 * OneDrive support has been added. A copy of your data can be saved to the main directory of your OneDrive account. This can be copied other places if you want, but will only pull from that location.
 * Main page now shows the latest 20 events in each category instead of 10.
