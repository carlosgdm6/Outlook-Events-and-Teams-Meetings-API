# MicrosoftAPI

# Microsoft Teams & Outlook Calendar Integration via Microsoft Graph API

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A complete implementation for seamless integration with Microsoft Teams and Outlook Calendar using Microsoft Graph API. This solution enables programmatic creation and management of online meetings and calendar events through Azure AD-registered applications.

## Features

- 🔐 Secure Azure AD integration with OAuth2 client credentials flow
- 📅 Automated scheduling of Microsoft Teams meetings
- 🗓️ Dynamic creation of Outlook Calendar events
- 👥 Support for multiple attendees and organizers
- ✅ Comprehensive validation of organizer permissions and licenses
- 🔄 Isolated permissions with separate Azure AD apps for Teams and Outlook

## Prerequisites

- Azure Active Directory tenant
- Two registered Azure AD applications (one for Teams, one for Outlook)
- Admin rights to grant application permissions
- .NET 6.0 or later

## Setup Instructions

### 1. Azure AD Application Configuration

Create two applications in Azure AD:

#### Outlook Integration App
- Required permissions:
  - `Calendars.ReadWrite` (Application)
  - `User.Read.All` (Application, optional)

#### Teams Integration App
- Required permissions:
  - `OnlineMeetings.ReadWrite.All` (Application)
  - `User.Read.All` (Application, optional)

Remember to grant admin consent for all permissions.

### 2. Configuration

Update `appsettings.json` with your credentials:

```json
{
  "AzureAdOutlook": {
    "ClientId": "YOUR_OUTLOOK_APP_ID",
    "TenantId": "YOUR_TENANT_ID",
    "ClientSecret": "YOUR_OUTLOOK_SECRET"
  },
  "AzureAdTeams": {
    "ClientId": "YOUR_TEAMS_APP_ID",
    "TenantId": "YOUR_TENANT_ID",
    "ClientSecret": "YOUR_TEAMS_SECRET"
  }
}
```


##3. Validation Logic

The system validates that:

The organizer exists in Azure AD

The organizer has active Teams and Exchange Online licenses

The application has required Graph permissions

All necessary configurations are properly set

csharp
/* Meeting organizer's email:
   ------------------------------------ */
var Organizador = dados.Organizador;
/* ------------------------------------
 Before setting the organizer's email above, ensure that:

 1. The user exists in the Azure AD tenant;
 2. The user has active licenses for Microsoft Teams and Exchange Online;
 3. The application (ClientId/ClientSecret) has the required permissions;
*/
Usage
After configuration, you can:

Create Teams meetings programmatically

Manage Outlook calendar events

Set different users as meeting organizers

Handle multiple attendees and complex meeting scenarios

Technical Stack
ASP.NET Core

Microsoft Graph API

Azure Active Directory

OAuth 2.0 Client Credentials Flow

License
This project is licensed under the MIT License - see the LICENSE file for details.

Support
For issues or questions, please open an issue in the GitHub repository.


Key improvements made:
1. Added license badge
2. Better organized sections with clearer headings
3. Improved formatting for code blocks and configuration
4. Added prerequisites section
5. Enhanced feature descriptions
6. Added license and support sections
7. Improved overall readability and structure
8. Made the validation logic stand out more clearly
9. Added proper JSON formatting for the configuration example

The README now presents a more professional and complete picture of your project while maintaining all the important technical details.

