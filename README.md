## Predmet-projekat

# Introduction
Microservice web application designed for power system outage reporting and handling, created as a base for my BSc thesis. Users can report power outages in their area and first available dispatcher will start handling it by creating incident report and passing it to teams on the field.

Other various features are available such as:
  - incident statistics charts
  - user blocking/approving,
  - map view of all power system elements and incidents
  - document upload with virus scan
  - facebook/google log in
  - and many more..

Data consistency in distributed transactions is enforced with orchestration SAGA pattern implemented with event driven messages.
Whole system is orchestrated with docker-compose.

More info can be found in my BSc thesis in Serbian https://github.com/AnLong98/Predmet-projekat/blob/master/diplomski%20finalni.pdf

# Technologies
- Angular 11
- .NET Core 3.1
- SQL server
- DAPR .NET
- RabbitMQ
- Docker
- Docker compose

# How to run this thing?

# 1. Download and unzip master branch

# 2. Run front end app

- Open power-system-web-app project in VS code
- Open new terminal
- Type npm install
- After instalation type ng serve
- Follow instructions here to trust SSL cert https://www.pico.net/kb/how-do-you-get-chrome-to-accept-a-self-signed-certificate

# 3. Run back end (docker desktop required)

- Open SmartEnergyV2 solution
- Set docker-compose as startup project
- Run it
- Use SSMS to connect to localhost,1401 with password Your+password123 
- Use UsersDbSeed.sql script to seed database
- Repeat process for localhost,1402 with PhysicalDbSeed.sql and same password
- Repeat process for localhost,1403 with DocumentsDbSeed.sql and same password

# 4. Use existing user account to log in

- stele98 password 1234 as Dispatcher
- sveto98 password reklio90 as Admin
- nizda98 password reklio90 as Admin
- Or register a new account...

## Final notes

- Project is not maintained and is used as proof of concept, some features may not work properly and some external APIs support may be terminated in the future.
- Facebook log in will be terminated after 17/7/2021.
- Dockerized version of project does not (yet) support virus scanning for files. Support is maintained in monolith version.
- On older PC's back end app  may take up to 15 minutes to run, with about 16GB of RAM required.


Copyright @Predrag Glavas & Nikola Mijonic
