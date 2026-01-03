# Jellyfin PDF Cover Plugin

The Jellyfin PDF Cover plugin automatically extracts and generates cover images from your PDF files to enhance your library's visual experience.

> [!IMPORTANT]
> Currently, this plugin has only be tested on **Linux (x64) and Windows (x64)** environments.

## 🚀 Installation

### 1. Via Plugin Repository (Recommended)
You can install and receive automatic updates by adding the repository to your Jellyfin server:

1. Navigate to the **Jellyfin Dashboard**.
2. Go to **Plugins** and click the **Manage Repositories** button.
3. Click the **+ New Repository** button and enter the following details:
   - **Repository Name**: PDFCover Plugin
   - **Repository URL**:
     ```
     https://raw.githubusercontent.com/unfedorg/jellyfin-plugin-pdfcover/main/plugin-repo/manifest.json
     ```
4. Back to the **Plugin Catalog**, find **"PDFCover"**, and click install.
5. Restart your Jellyfin server.

### 2. Manual Installation
1. Download the latest `pdfcover_x.x.x.x.zip` from the Releases page.
2. Create a folder named `PDFCover` inside the `plugins` directory of your Jellyfin server.
3. Extract the contents of the ZIP file into the newly created `PDFCover` folder.
4. Restart your Jellyfin server.

## ⚙️ Configuration and Usage

1. After installation, go to your **Library** settings.
2. Select a library with the **Books** content type.
3. Scroll down to the **Image fetchers** section.
4. Locate **"PDF Cover Generator"** and check the box to enable it.
5. Save your settings and run a **Scan Library** (refreshing metadata) to generate covers for your PDF files.
