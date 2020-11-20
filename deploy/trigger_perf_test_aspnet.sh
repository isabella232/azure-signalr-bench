#!/bin/bash

. ./vsts_trigger_jenkins_perf_test.sh

# aspnet echo
vsts_trigger_jenkins_perf_test \
"$PERF_DEFAULT_GROUP_TYPE" \
"$PERF_RUN_INDEXES" \
"$PERF_BUILD_URL" \
"$PERF_USER_NAME" \
"$API_TOKEN" \
"$PERF_ASRS_LOCATION" \
"$PERF_UNITS" \
"echo" \
"$PERF_DEFAULT_TRANSPORT_TYPE" \
"$PERF_DEFAULT_PROTOCOL" \
"$PERF_DEFAULT_MESSAGE_SIZE" \
"$PERF_VM_LOCATION" \
"$PERF_VM_IMAGE_ID" \
"$PERF_VNET_RESOURCE_GROUP" \
"$PERF_VNET_NAME" \
"$PERF_VNET_SUBNET" \
"$PERF_CLIENT_VM_COUNTS" \
"$PERF_SERVER_VM_COUNTS" \
"$PERF_BUILD_TYPE" \
"$RELEASE_NAME" \
"$RELEASE_ID" \
"$PERF_ASPNET" \
"$PERF_SERVICE_MODE" \
"$PERF_DURATION"

# aspnet broadcast
vsts_trigger_jenkins_perf_test \
"$PERF_DEFAULT_GROUP_TYPE" \
"$PERF_RUN_INDEXES" \
"$PERF_BUILD_URL" \
"$PERF_USER_NAME" \
"$API_TOKEN" \
"$PERF_ASRS_LOCATION" \
"$PERF_UNITS" \
"broadcast" \
"$PERF_DEFAULT_TRANSPORT_TYPE" \
"$PERF_DEFAULT_PROTOCOL" \
"$PERF_DEFAULT_MESSAGE_SIZE" \
"$PERF_VM_LOCATION" \
"$PERF_VM_IMAGE_ID" \
"$PERF_VNET_RESOURCE_GROUP" \
"$PERF_VNET_NAME" \
"$PERF_VNET_SUBNET" \
"$PERF_CLIENT_VM_COUNTS" \
"$PERF_SERVER_VM_COUNTS" \
"$PERF_BUILD_TYPE" \
"$RELEASE_NAME" \
"$RELEASE_ID" \
"$PERF_ASPNET" \
"$PERF_SERVICE_MODE" \
"$PERF_DURATION"

# aspnet group
vsts_trigger_jenkins_perf_test \
"small tiny" \
"$PERF_RUN_INDEXES" \
"$PERF_BUILD_URL" \
"$PERF_USER_NAME" \
"$API_TOKEN" \
"$PERF_ASRS_LOCATION" \
"$PERF_UNITS" \
"sendToGroup" \
"$PERF_DEFAULT_TRANSPORT_TYPE" \
"$PERF_DEFAULT_PROTOCOL" \
"$PERF_DEFAULT_MESSAGE_SIZE" \
"$PERF_VM_LOCATION" \
"$PERF_VM_IMAGE_ID" \
"$PERF_VNET_RESOURCE_GROUP" \
"$PERF_VNET_NAME" \
"$PERF_VNET_SUBNET" \
"$PERF_CLIENT_VM_COUNTS" \
"$PERF_SERVER_VM_COUNTS" \
"$PERF_BUILD_TYPE" \
"$RELEASE_NAME" \
"$RELEASE_ID" \
"$PERF_ASPNET" \
"$PERF_SERVICE_MODE" \
"$PERF_DURATION"