# Brevium Scheduler

A lightweight **C#/.NET 9 console application** that automatically schedules doctor appointments through Brevium’s REST API.  
It demonstrates clean OOP design, async programming, and layered architecture.

---

## Overview

The program:
1. Resets the test system  
2. Loads the initial schedule and appointment requests  
3. Applies scheduling constraints to find valid slots  
4. Posts new appointments via the API  
5. Retrieves and prints the final schedule  

**Tech Stack:**  
- C# / .NET 9  
- Async / Await  
- RESTful API (HttpClient)  
- OOP design with interfaces and layering  

---

## UML Diagram (ERD-style)

```mermaid
erDiagram
    PROGRAM ||--|| SCHEDULINGCOORDINATOR : controls_flow
    SCHEDULINGCOORDINATOR ||--|| SCHEDULINGRULES : applies_rules
    SCHEDULINGCOORDINATOR ||--|| APIFACADE : uses_API
    APIFACADE ||--|| ISCHEDULINGAPI : implements
    APIFACADE ||--|| MODELS : reads_writes_data

    PROGRAM {
        void Main()
    }

    SCHEDULINGCOORDINATOR {
        void RunAsync()
    }

    SCHEDULINGRULES {
        bool FindValidSlot()
    }

    APIFACADE {
        Task StartTestAsync()
        Task GetScheduleAsync()
        Task PostAppointmentAsync()
    }

    ISCHEDULINGAPI {
        interface_methods string
    }

    MODELS {
        string Appointment
        string AppointmentRequest
        string AppointmentCreate
    }