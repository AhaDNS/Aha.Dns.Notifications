# Aha.Dns.Notifications

AhaDNS automated social media poster.

## Intended usecase

The solution in this repository is created for [AhaDNS](https://ahadns.com). The purpose of the solution is to post content on our social media platforms on [Twitter](https://twitter.com/ahadns_official) and [Telegram](https://t.me/ahadns_announcements).

## Technologies and frameworks used

- [Azure Functions](https://azure.microsoft.com/en-us/services/functions/)
- Programming language C#

## Projects in this repository

Following is an overview of all projects in this repository and their function.

### Aha.Dns.Notifications.CloudFunctions

An [Azure Function](https://azure.microsoft.com/en-us/services/functions/) project responsible for automatically post content on our social media accounts. Here's a brief description of each function in the project and it's intended use case.

#### **DailyRequestStatisticsPoster-function** (automatically triggered every day at 19:00 W. Europe Standard Time)

1. Retrieves DNS statistics from `Aha.DNs.Statistics.CloudFunctions` API [SOURCE](https://github.com/AhaDNS/Aha.Dns.Statistics/blob/master/src/Aha.Dns.Statistics.CloudFunctions/Functions/SummarizedStatisticsApi.cs).
2. Post statistics in all available NotificationClients (currently Twitter & Telegram).

## How to handle AppSettings?

1. In `Aha.Dns.Notifications.CloudFunctions`, rename `example.settings.json` to `local.settings.json` and configure all example values.
