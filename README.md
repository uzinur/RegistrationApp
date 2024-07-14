# RegistrationApp

## Setting Up Multiple Startup Projects

To ensure that both the server and client projects start correctly, follow these steps:

1. Open the solution in Visual Studio.
2. In the **Solution Explorer**, right-click the solution name and select **"Set Startup Projects..."**.
3. In the **Solution Property Pages** dialog, go to the **"Common Properties"** tab and select **"Startup Project"**.
4. Set the selector to **"Multiple startup projects"**.
5. Find and set the following projects to start:
    - `RegistrationApp.WebAPI` - select **"Start"**
    - `registrationapp.client` - select **"Start"**
6. Ensure that `RegistrationApp.WebAPI` is listed above `registrationapp.client` to maintain the correct startup order.
7. Click **OK** to save the settings.

### Note

These settings are stored in a user options file (.suo) which is not included in version control. Each developer working with the repository will need to perform these steps to set up multiple startup projects.

## Running and Testing

- To run the solution, use **F5** or **Ctrl+F5** in Visual Studio.
- Ensure that both projects start correctly and interact with each other as expected.

### Additional Information

- **RegistrationApp.WebAPI**: The server-side project implementing the API.
- **registrationapp.client**: The client-side project that interacts with the API.

