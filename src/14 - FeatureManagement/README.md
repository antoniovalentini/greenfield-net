# Feature Management

Simple example of how to use Microsoft's [Feature Management](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-dotnet-reference) in a console application.

The example has 2 features configured in the `appsettings.json` file:
- `MyAlwaysDisabled`: This feature is always disabled
- `MyBuiltInFiftyPercentFeature`: This feature is enabled 50% of the time and it used the built-in `Percentage` filter

```json
{
  "FeatureManagement": {
    "MyBuiltInFiftyPercentFeature": {
      "EnabledFor": [
        {
          "Name": "Microsoft.Percentage",
          "Parameters": {
            "Value": 50
          }
        }
      ]
    },
    "MyAlwaysDisabled" : false
  }
}
```

Console output:
```
Feature management test

Feature: MyAlwaysDisabled
Enabled: 0 - Disabled: 1000

Feature: MyBuiltInFiftyPercentFeature
Enabled: 492 - Disabled: 508
```

As you noticed, the feature field can be set to both an `object` (Feature Filter) or `boolean` (On/Off) value. More info can be found in this [json schema](https://github.com/microsoft/FeatureManagement-Dotnet/blob/9c8487599508c4ffe170b25cadb5751af312cf16/schemas/FeatureManagement.Dotnet.v1.0.0.schema.json).
