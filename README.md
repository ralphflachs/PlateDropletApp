# Plate Droplet Data Display Application

Welcome to the **Plate Droplet Data Display** application! This WPF application allows users to load plate droplet data from JSON files, visualize the data in a grid format, and adjust droplet count thresholds to identify wells with low droplet counts.

---

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Clone the Repository](#clone-the-repository)
  - [Install NuGet Packages](#install-nuget-packages)
  - [Build and Run the Application](#build-and-run-the-application)
- [Project Structure](#project-structure)
- [Usage](#usage)
  - [Loading Plate Data](#loading-plate-data)
  - [Adjusting Droplet Threshold](#adjusting-droplet-threshold)
- [Configuration](#configuration)

---

## Features

- **Load Plate Data**: Import plate droplet data from JSON files.
- **Dynamic Visualization**: Display wells in a grid layout corresponding to the plate's dimensions.
- **Threshold Adjustment**: Set droplet count thresholds to identify wells with low droplet counts.
- **Responsive UI**: Asynchronous operations ensure the UI remains responsive during data loading.
- **MVVM Architecture**: Implements the Model-View-ViewModel pattern for maintainable and testable code.

---

## Prerequisites

- **Visual Studio 2019 or later** with WPF development workload.
- **.NET Framework 4.6.1** or higher.
- **NuGet Package Manager** for managing dependencies.

---

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/ralphflachs/PlateDropletApp.git
```

### Install NuGet Packages

The project relies on several NuGet packages. You can restore these packages using Visual Studio or via the Package Manager Console.

**Using Visual Studio:**

- Open the solution file `PlateDropletApp.sln`.
- Visual Studio will prompt you to restore missing NuGet packages. Click **Restore**.

### Build and Run the Application

- **Build** the solution by pressing `Ctrl+Shift+B` or going to **Build > Build Solution**.
- **Run** the application by pressing `F5` or clicking the **Start** button.

---

- **Models**: Contains data models (`Plate`, ...).
- **Services**: Contains service classes (`PlateDataService`, ...).
- **ViewModels**: Contains ViewModel classes following MVVM pattern.
- **Views**: Contains XAML views and their code-behind files.
- **Converters**: Contains value converters for data binding in XAML.
- **Resources**: Contains styles for views.
- **App.xaml**: Application definition.
- **App.xaml.cs**: Application startup logic.

---

## Usage

### Loading Plate Data

1. Click on the **Select Plate Data** button.
2. An open file dialog will appear. Navigate to your JSON data file and select it.
3. The application will asynchronously load and display the plate data.

### Adjusting Droplet Threshold

1. Enter a new droplet threshold value in the **Droplet Threshold** textbox.
2. Click the **Update** button to apply the new threshold.
3. Wells with droplet counts below the threshold will be highlighted accordingly.

---

## Configuration

The application uses an `App.config` file to store configuration settings.

- **Default Droplet Threshold**: You can set the default droplet threshold value in the `App.config` file.

  ```xml
  <appSettings>
    <add key="DefaultDropletThreshold" value="100" />
  </appSettings>
  ```

---

## Notes

- **Data Format**: Ensure your JSON data files match the expected format for successful parsing.
- **Plate Dimensions**: The application supports plates with 96 or 48 wells. Other well counts will result in an error.