# SmartSupport

AI-powered support ticket management system built with React, ASP.NET Core, SQL Server, and Python/scikit-learn.

## Architecture

React frontend → ASP.NET Core REST API → SQL Server
                               ↘ Python FastAPI ML service

## Target stack

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- React + Vite
- Python + FastAPI
- pandas / NumPy / scikit-learn

## Build order

1. Ticket CRUD API + database
2. Authentication and roles
3. React dashboard
4. ML dataset + training pipeline
5. FastAPI `/predict`
6. ASP.NET-to-ML integration
7. UI polish + deployment
