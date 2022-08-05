# Dockerfile for DotNetDev container
#FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0-core-tools
FROM arm64v8/debian:bullseye-slim

ARG DOTNET_BASE_URL=https://download.visualstudio.microsoft.com/download/pr
ARG DOTNET_DOWNLOAD=33389348-a7d7-41ae-850f-ec46d3ca9612/36bad11f948b05a4fa9faac93c35e574/dotnet-sdk-6.0.302-linux-arm64.tar.gz


# Basic setup
RUN mkdir -p /workspace/.vscode \
    && echo "set -o vi" >> /root/.bashrc


# Copy VS Code files
COPY ./.vscode /workspace/.vscode


# Install standard packages
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    curl \
    gnupg \
    libicu67 \
    software-properties-common \
    vim \
    wget


# Install DotNet
#RUN wget https://download.visualstudio.microsoft.com/download/pr/33389348-a7d7-41ae-850f-ec46d3ca9612/36bad11f948b05a4fa9faac93c35e574/dotnet-sdk-6.0.302-linux-arm64.tar.gz -O /tmp/dotnet-sdk.tar.gz \
RUN mkdir -p /root/.dotnet \
    && wget ${DOTNET_BASE_URL}/${DOTNET_DOWNLOAD} -O /tmp/dotnet-sdk.tar.gz \
    && tar zxf /tmp/dotnet-sdk.tar.gz -C /root/.dotnet \
    && echo "export PATH=$PATH:/root/.dotnet" >> /root/.bashrc


# Install Azure Functions Core Tools
RUN echo "deb [arch=amd64,armhf,arm64] https://packages.microsoft.com/debian/11/prod bullseye main" >> /etc/apt/sources.list.d/microsoft-prod.list \
    && wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg \
    && mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
