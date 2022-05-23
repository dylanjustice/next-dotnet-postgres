FROM envoyproxy/envoy:v1.21-latest

COPY dotnet-api.yaml /etc/envoy/dotnet-api.yaml
CMD /usr/local/bin/envoy -c /etc/envoy/dotnet-api.yaml