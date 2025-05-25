# MicrosoftAPI

# 📅 Microsoft Teams & Outlook Calendar Integration via Microsoft Graph API

This project is a complete implementation that enables seamless integration with **Microsoft Teams** and **Outlook Calendar** using the **Microsoft Graph API**. It is designed for creating and managing online meetings and calendar events programmatically through **Azure AD-registered applications**.

---

## 💼 Use Case

This system is ideal for enterprise environments that require:
- Automated scheduling of **Microsoft Teams meetings**
- Dynamic creation of **Outlook Calendar events**
- Integration into internal applications or bots that manage appointments and team sessions

---

## ⚙️ How It Works

The solution uses **two separate Azure AD applications** to isolate permissions and responsibilities:

1. **Outlook Integration App**  
   - Used for calendar management via Microsoft Graph  
   - Application permissions:  
     - `Calendars.ReadWrite`  
     - `User.Read.All` *(optional, for user existence validation)*

2. **Teams Integration App**  
   - Used to create and manage Teams meetings  
   - Application permissions:  
     - `OnlineMeetings.ReadWrite.All`  
     - `User.Read.All` *(optional)*

The app uses the **client credentials flow** (Client ID + Client Secret) to authenticate securely with Azure.

---

## 🧠 Key Features

- 🔐 Secure integration using Azure AD + OAuth2
- 🧑‍💼 Set different users as meeting organizers
- 📆 Create calendar events with full control over subject, attendees, time and location
- 📎 Support for multiple users and dynamic token generation
- ✅ Validates if the organizer has:
  - An active Teams & Exchange Online license
  - Is present in Azure AD
  - The app has required Graph permissions

---

## 🔧 Setup Instructions

### 1. Clone the Repository

bash
git clone https://github.com/your-username/your-repository.git

## 2. Configure Azure AD Apps

Create two apps in Azure AD:

- OutlookIntegration
- TeamsIntegration

Set the client secrets and configure the required API permissions as explained below.

---

 3. Configure appsettings.json

Create or update your `appsettings.json` with the following structure, replacing placeholders with your actual app IDs, tenant ID, and secrets:

json
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

---

## 🔑 Required Azure AD Permissions

| App                | Permission Type | Permission                    |
|--------------------|-----------------|-------------------------------|
| Outlook Integration | Application     | Calendars.ReadWrite           |
|                    | Application     | User.Read.All (optional)      |
| Teams Integration   | Application     | OnlineMeetings.ReadWrite.All  |
|                    | Application     | User.Read.All (optional)      |

⚠️ Important: After adding these permissions in the Azure Portal, don't forget to grant admin consent to allow the application to use them.

---

## 🧪 Validation Logic (In Code)

```csharp
/* Meeting organizer's email:
   ------------------------------------ */
var Organizador = dados.Organizador;
/* ------------------------------------
 Before setting the organizer's email above, ensure that:

 1. The user exists in the Azure AD tenant;
 2. The user has active licenses for Microsoft Teams and Exchange Online;
 3. The application (ClientId/ClientSecret) has the following Application permissions:
    • OnlineMeetings.ReadWrite.All
    • Calendars.ReadWrite
 4. If changing the organizer dynamically, validate that all the above are met.
*/

---

## 📚 Technologies Used

✅ ASP.NET Core

✅ Microsoft Graph API

✅ Azure Active Directory (Azure AD)

✅ OAuth 2.0 (Client Credentials Grant)

✅ JSON Configuration

---
