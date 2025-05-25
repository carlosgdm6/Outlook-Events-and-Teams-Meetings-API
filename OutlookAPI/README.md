# Azure AD Configuration for Outlook and Teams Integration

This project uses two separate Azure Active Directory (Azure AD) applications for integration with:

- Microsoft Outlook (Calendar)
- Microsoft Teams (Meetings)

---

## 🔧 1. Create the Azure AD Applications

Go to [Azure Portal](https://portal.azure.com):

1. Search for **"App registrations"** and click on **"New registration"**
2. Create two separate applications:
   - **OutlookIntegration**
   - **TeamsIntegration**

For both apps:
- Choose your **Tenant only** as the supported account type (unless you have specific needs)
- Set **Redirect URI** if applicable (e.g., for OAuth2 flows)

---

## 🔑 2. API Permissions

### 🔷 OutlookIntegration App

Required **Application** permissions (Microsoft Graph):
- `Calendars.ReadWrite`  
- `User.Read.All` *(optional, only if you need to validate user existence)*

> After adding permissions, don't forget to **click "Grant admin consent"**.

---

### 🔷 TeamsIntegration App

Required **Application** permissions (Microsoft Graph):
- `OnlineMeetings.ReadWrite.All`  
- `User.Read.All` *(optional, only if you need to validate user existence)*

> Again, **click "Grant admin consent"** after adding the permissions.

---

## 🔐 3. Client Credentials

For each app:

1. Go to **Certificates & secrets**
2. Click **"New client secret"**
3. Store the generated secret securely — you'll need it in your `appsettings.json`.

---

## 🧪 4. Validating the Organizer Email (in code)

Before using a user as the meeting organizer:
- Ensure the user:
  - Exists in the Azure AD tenant
  - Has an active **Teams** and **Exchange Online** license
- Ensure the **application** (via Client ID and Secret) has all required **Application permissions**
- If setting the organizer dynamically, perform validation to confirm the above

---

## 📁 Example `appsettings.json` Structure

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
