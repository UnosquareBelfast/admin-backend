# AdminCore

AdminCore is currently an internal solution for Unosquare to manage employee holidays.

There are currently 3 parts to the AdminCore project:
1. Web-App - Is the front-end for the AdminCore web-app.
2. NativeApp - Is the front-end for the AdminCore android and iOS apps.
3. BackEnd - The back-end to the project. Web-App and NativeApp run off this.

## Getting up and running
Before following the steps be sure to clone this repository to the local machine.

### BackEnd
**Note: If you're on a Windows machine that doesn't have the pro version of the operating system, follow the non-pro instructions.**

1. Download & Install [Docker](https://www.docker.com/products/docker-desktop).
2. Download & Install [PostgreSQL](https://www.postgresql.org/download/)
3. Login to docker: Try `docker login` first, or if using bash `winpty docker login`. Note it asks for **username**, not email.
4. Finally, run the project with `docker-compose up`

#### Non-Pro Windows:

1. Download & Install [Docker Toolbox](https://docs.docker.com/toolbox/toolbox_install_windows/).
2. Follow the docs in the above guide until you get the docker command console open. Enter all docker commmands here.
3. Follow the standard instructions from point 2.

#### Mac (Make)
For mac users you can us Make to build Docker images.
1. `(make | nmake) build_admin_core version=latest`
2. `(make | nmake) build_cors_proxy version=latest`
3. `(make | nmake) version=latest`

## How to update the back end
Currently it's a bit of handling to update the backend. Here's how to it. Run the following commands in `/BackEnd/`. I've found that I need to use powershell or git bash for some of the following commands.

1. De-compose the BackEnd `docker-compose down`.
2. Delete all containers `docker rm -f $(docker ps -a -q)`
3. Delete all volumes `docker volume rm $(docker volume ls -q)`
4. Remove images: `docker rmi unosquare/admincore unosquare/cors-proxy`
4. Finally, run the project with `docker-compose up`

## Back end troubleshooting
- If you're experiencing problems after running `docker-compose up` it may be due to PostgresQL running locally. Use task manager or the mac equivilent to shut down all the PostgresQL processes.
