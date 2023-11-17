# Dashboard-backend

## Suggestions for a good README

Every project is different, so consider which of these sections apply to yours. The sections used in the template are
suggestions for most open source projects. Also keep in mind that while a README can be too long and detailed, too long
is better than too short. If you think your README is too long, consider utilizing another form of documentation rather
than cutting out information.

## Name

Dashboard-backend

## Description

This API project is the backend for the Tracking Fellowship application.
It proposes all the functionality required for the application and handles the user management.
It also serves as a bridge for all the independent supported game modules.

## Installation

### Run Locally setup

* Have Docker Desktop [install](https://www.docker.com/products/docker-desktop/)
* Set Environment variable, make sure {username} and {password} correspond to the one use in the database. **THIS IS ON
  YOUR MACHINE**     
  * **DB_CONNECTION_STRING**=Server=localhost;Port=3306;Database=TheTrackingFellowship;Uid={_username_};Pwd={_password_};
  * **JWT_SECURITY_KEY**={_A Security Key_};
  * **TFT_BASE_URL**={_Teamfight Tactics module address_};
  * **LOL_BASE_URL**={_League Of Legends module address_};
  * **LOR_BASE_URL**={_Legends Of Runeterra module address_};

* Run a docker container with MySql image
  **docker run --name TheTrackingFellowshipDB --env=MYSQL_USER={username} --env=MYSQL_PASSWORD={password}
  --env=MYSQL_ROOT_PASSWORD=password --env=MYSQL_DATABASE=TheTrackingFellowship -p 3306:3306 --runtime=runc -d mysql:
  latest**
* Follow in
  detail [Database setup](https://docs.google.com/document/d/1p69wIILEzEz_yPN9W7Gb8XiaFdJTpW8ecPyv5YhtKA8/edit?usp=sharing)

## Usage
| Type  | Controller | Route                 | Request Model                                     | Response Model                                     |     
|-------|------------|-----------------------|---------------------------------------------------|----------------------------------------------------|
| POST  | Auth       | register              | [UserRegisterRequest](#userregisterrequest)       | [AuthenticateResponse](#authenticateresponse)      |    
| POST  | Auth       | authenticate          | [LoginRequest](#loginrequest)                     | [AuthenticateResponse](#authenticateresponse)      |     
| PATCH | Auth       | refresh-token         | [RefreshTokenExchange](#refreshtokenexchange)     | [RefreshTokenExchange](#refreshtokenexchange)      |     
| POST  | Auth       | logout                | [RefreshTokenExchange](#refreshtokenexchange)     | N/A                                                |     
| POST  | User       | update-gamertags      | [UpdateGamertagsRequest](#updategamertagsrequest) | ICollection<[GamertagResponse](#gamertagresponse)> |     
| POST  | User       | update-password       | [UpdatePasswordRequest](#updatepasswordrequest)   | N/A                                                |     
| GET   | User       | get-gamertags         | N/A                                               | ICollection<[GamertagResponse](#gamertagresponse)> |
| GET   | LoL        | get-stats-for-player  | N/A                                               | String (model from LOL-module)                     |
| GET   | LoL        | get-champion-win-rate | String champName                                  | String (model from LOL-module)                     |
| GET   | LoL        | get-lane-win-rate     | String lane                                       | String (model from LOL-module)                     |
| GET   | LoR        | get-stats-for-player  | N/A                                               | String (model from LOR-module)                     |
| GET   | TFT        | get-summoner          | N/A                                               | String (model from TFT-module)                     |
| GET   | TFT        | get-matches           | N/A                                               | String (model from TFT-module)                     |

## Models

### Requests

#### UserRegisterRequest

| Name     | Type   | Required |
|----------|--------|----------|
| username | string | true     |
| password | string | true     |
| email    | string | true     |

#### UpdatePasswordRequest

| Name        | Type   | Required |
|-------------|--------|----------|
| oldPassword | string | true     |
| newPassword | string | true     |

#### UpdateGamertagsRequest

| Name             | Type                                             | Required |
|------------------|--------------------------------------------------|----------|
| gamertagRequests | ICollection<[gamertagRequest](#gamertagrequest)> | true     |

#### GamertagRequest

| Name       | Type          | Required |
|------------|---------------|----------|
| gamertagId | string        | true     |
| tag        | string        | true     |
| game       | [Game](#game) | true     |

#### LoginRequest

| Name     | Type   | Required |
|----------|--------|----------|
| username | string | true     |
| password | string | true     |

### Exchange (Used for request and response)
#### RefreshTokenExchange

| Name            | Type   | Required |
|-----------------|--------|----------|
| jwtToken        | string | true     |
| refreshTokenKey | string | true     |

### Response

#### AuthenticateResponse

| Name     | Type   |
|----------|--------|
| username | string |
| userId   | ulong  |
| email    | string |
| jwtToken | string |

#### GamertagResponse

| Name       | Type   |
|------------|--------|
| game       | int    |
| gameName   | string |
| gamertagId | ulong  |
| tag        | string |

### Enum

#### Game

| Index | Value                |
|-------|----------------------|
| 0     | League Of Legends    |
| 1     | LEgends Of Runeterra |
| 2     | Teamfight Tactics    |

## Support

Tell people where they can go to for help. It can be any combination of an issue tracker, a chat room, an email address,
etc.

## Roadmap

If you have ideas for releases in the future, it is a good idea to list them in the README.

## Authors and acknowledgment

### Authors

* Catherine Bronsard
* David Goulet-Paradis
* Simon Lacroix
* Antoine Toutant

### Acknowledgment

* Mikaël Fortin, Project Supervisor

## License

For open source projects, say how it is licensed.

## Project status

In development 
