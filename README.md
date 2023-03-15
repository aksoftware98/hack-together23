# Magic Note - #Hack_Together
This project is built to participate in the Microsft's hackathon #Hack_Together 

[![Hack Together: Microsoft Graph and .NET](https://img.shields.io/badge/Microsoft%20-Hack--Together-orange?style=for-the-badge&logo=microsoft)](https://github.com/microsoft/hack-together)


[![Build and deploy ASP.Net Core app to an Azure Web App](https://github.com/aksoftware98/hack-together23/actions/workflows/azure-webapps-dotnet-core.yml/badge.svg)](https://github.com/aksoftware98/hack-together23/actions/workflows/azure-webapps-dotnet-core.yml)

![Magic Note - Hack Together](https://github.com/aksoftware98/hack-together23/blob/main/Assets/MagicNote%20Logo.png | width=130)

## Overview
Magic Note is an AI-powered application that allows your to plan your day smoothly and quickly without the need to open different apps like the calendar and the To-Do app and insert each of your items one by one. 
The app will have a simple text input where you can write all what you want to do for the next day (event, task, or a meeting with someone), after you submit your note, Magic Note with the help of AI and Microsoft Graph will understand the content and build a plan for you to review and make sure it aligns with what in your mind. 
Once you feel good and make the edits, submit the plan and Magic Note will add all the resources (events, meetings, and to-do tasks) and add them for you with one click. 

## Magic Note Availabitliy 
I have this idea in mind for long time but I was lazy to get it done, with the courage of the HackTogether it seems it's the good time to get it done. 
I will make this app available on all the platforms, but for this hackathon it will be available only as a native Windows app (because I want to learn WinUI 😁) and make it availalbe on **Microsoft Surface Duo** Android Dual Screen device. 
After the hackathon I will make the app available on all Android devices, iOS, Mac OSX, and Windows of course. 

The Windows app overview:
![WinUI](https://github.com/aksoftware98/hack-together23/blob/main/Assets/WinUI%20Project%20Overview/WinUI%20Project%20Overview.gif?raw=true)

## What Magic Note solves specifically
Most productive people would like to have the upcoming busy day planned carefully the night before. The process is needed but it's tough, why? Well! we have the tasks we want to do in mind but just thoughts, that's why we take notes and create To-Do lists. The problem happens we open the calendar and the To-Do lists app on our smart device and start adding some important events, meetings, tasks, or other reminders for the upcoming day.

How do users do it without Magic Note? Let's see the steps
The 10 things to do the next day (3 events, 2 meetings, 5 to-do items) All thoughts in the brain yet

Open the Calendar app on the phone
Navigate to the next day
Click Add New Event
Populate the title
Define the start time
Define the end time
Click save
Repeat the same for the other 2 events if the user doesn't forget any.
Repeat the same for the meetings
Open the To-Do app
Enter the description of the first to-do item
Hit save
Repeat the same for the remaining 4 items if the user still remembers all of them.
The process is boring and inefficient.

Writing down everything in mind helps us express our ideas and keep them saved outside our minds, many people write their diaries with a pen and a sheet of paper, that's quick and indestructible. But the paper won't send me a notification 1 hour before the event or send an email for the person I want to meet with if I typed the meeting on that piece of paper.

Here is where Magic Note comes into play.
Let's see how the same will be achieved 10x more efficiently using the Magic Note

## How Magic Note solved the problem 
The app is only a single big text box on the left, that's it all. The user starts typing what he/she is up to for the next day in their daily conversational English as if the user is writing his/her diary or a list of tasks on a sheet of paper. 
The writing is quick, with no text fields to fill, no dates to select, and no save buttons to click. List everything in mind in that text box. Once everything is done, the user clicks Plan.
Magic Note will send that text to the server, understand its content using AI (SEE BELOW FOR TECHNOLOGY DETAILS), and build a plan out of the ideas inside (creating an event at this time, creating a meeting with a person, adding a to-do item ..etc.). After the server prepares the plan, the user will see a set of cards each representing either an event to be added to the calendar, scheduling a meeting with someone, or adding a to-do item.
Instead of the user having to open every app (Calendar, Teams, and To-Do) to insert them, the user now has a plan in front and he/she should decide if this is correct and make adjustments if needed. Once the user is satisfied with the plan, click Submit and the Magic Note will use Microsoft Graph to populate all this stuff.
The user starts the next day with the To-Do app populated, the calendar full of the events and the meeting needed to be done in that day.

## Magic Note Components
Technologies Used
Because this Hackathon is for fun, learning, and everything Microsoft and because I'm the biggest Microsoft fan ever. I decided to build this demo strictly for MICROSOFT PEOPLE (Users and Developers) 😂😎
The is built with .NET and Azure and divided into the following:

Client-side desktop: WinUI app for Windows 10 & 11
Client-side mobile: .NET MAUI app for Android only and for Microsoft Surface Duo specifically (The app suits the two screens approached perfectly, write on the left and see the plan on the right)
Server-side: Set of ASP.NET Core Minimal API
Microsoft Graph API
For language understanding AI: I used Azure Conversation Language Service

## Project Progress & Goals
Following is the table of the tasks I want to get done for the hackathon and for the releasing of it after the app is reviewed and want to make it publicly available

| Status | Task | Target | 
|--|--|--|
| :white_check_mark: | Having AI models supprot understanding (tasks, meetings, and events) | Hackathon |
| :white_check_mark: | Integrate with Microsoft Graph for (tasks, calendar events, and contacts) | Hackathon |
| :white_check_mark: | Release the basic version of WinUI project for Windows 10, 11 | Hackathon |
| :white_square_button: | Release the basic version of the MAUI project to run on Microsoft Surface Duo | Hackathon | 
| :white_square_button: | Implement efficient validation for the request (client and server side) | Hackathon |
| :white_square_button: | Refactor the code of the analyzing logic on the server | Hackathon |
| :white_square_button: | Support Sign-Out | v1 public |
| :white_square_button: | Add voice recognition to fill out the note instead of writing | v1 public |
| :white_square_button: | Add text extraction from image to support users who prefer writing on paper | v1 public |

**The target of the project for the hackathon to be fully available and functional is March 14, 2023**
