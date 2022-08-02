# Dockerfile for DotNetDev container
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0-core-tools


# Basic setup
RUN mkdir -p /workspace/.vscode \
    && echo "set -o vi" >> /root/.bashrc


# Copy VS Code files
COPY ./.vscode /workspace/.vscode


# Install standard packages
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    curl \
    software-properties-common \
    vim \
    wget
