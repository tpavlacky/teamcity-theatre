#####################
#                   #
# MULTI STAGE BUILD #
#                   #
#####################


#########################
                       
# STAGE 1: INSTALL NODE #
                        
#########################
FROM alpine:3.5 AS installnode
WORKDIR /

# install node
RUN apk add --no-cache nodejs-current tini

# Set tini as entrypoint
ENTRYPOINT ["/sbin/tini", "--"]

########################
                      
# STAGE 2: NPM INSTALL #
                      
########################
FROM installnode AS npminstall

# copy bare minimum files needed to restore NPM packages
COPY ./src/TeamCityTheatre.Web/package.json ./TeamCityTheatre.Web
COPY ./src/TeamCityTheatre.Web/package-lock.json ./TeamCityTheatre.Web

WORKDIR /TeamCityTheatre.Web

# install node packages
RUN npm install

########################
                      
# STAGE 3: NPM BUILD   #
                       
########################
FROM npminstall as npmbuild
WORKDIR /

# take all files from stage 2
COPY . .

# and everything else needed to build the frontend
COPY ./src/TeamCityTheatre.Web/tsconfig.json ./TeamCityTheatre.Web
COPY ./src/TeamCityTheatre.Web/webpack.config.js ./TeamCityTheatre.Web
COPY ./src/TeamCityTheatre.Web/Views/. ./TeamCityTheatre.Web/Views

# .. and build the frontend
WORKDIR /TeamCityTheatre.Web
RUN npm run build:release

#############################
                       
# STAGE 4: DOT NET RESTORE  #
                       
#############################
FROM microsoft/dotnet:2.1-sdk AS dotnetrestore
WORKDIR /

# copy bare minimum files needed to restore dot net packages
COPY src/TeamCityTheatre.sln /
COPY src/TeamCityTheatre.Web/TeamCityTheatre.Web.csproj /TeamCityTheatre.Web
COPY src/TeamCityTheatre.Core/TeamCityTheatre.Core.csproj /TeamCityTheatre.Core
RUN dotnet restore

#############################
                       
# STAGE 5: DOT NET PUBLISH  #
                       
#############################
FROM dotnetrestore AS dotnetpublish
WORKDIR /

# copy all files from dot net restore
COPY . .

# copy all files from npm build
COPY --from=npmbuild . .

# copy everything else
COPY src/TeamCityTheatre.Core/. ./TeamCityTheatre.Core
COPY src/TeamCityTheatre.Web/. ./TeamCityTheatre.Web

# publish
RUN dotnet publish "/TeamCityTheatre.Web/TeamCityTheatre.Web.csproj" --verbosity normal --configuration Release --output "/Output"

#############################
                       
# STAGE 6: RUN APPLICATION  #
                       
#############################
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /
COPY --from=dotnetpublish /Output .
ENTRYPOINT ["dotnet", "TeamCityTheatre.Web.dll"]