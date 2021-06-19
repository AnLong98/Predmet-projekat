# Predmet-projekat

How to run this thing?

## 1. Download and unzip master branch

## 2. Run front end app

- Open power-system-web-app project in VS code
- Open new terminal
- Type npm install
- After instalation type ng serve
- Follow instructions here to trust SSL cert https://www.pico.net/kb/how-do-you-get-chrome-to-accept-a-self-signed-certificate

## 3. Run back end (docker desktop required)

- Open SmartEnergyV2 solution
- Set docker-compose as startup project
- Run it
- Use SSMS to connect to localhost,1401 with password Your+password123 
- Use UsersDbSeed.sql script to seed database
- Repeat process for localhost,1402 with PhysicalDbSeed.sql and same password
- Repeat process for localhost,1403 with DocumentsDbSeed.sql and same password

## 4. Use existing user account to log in

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
