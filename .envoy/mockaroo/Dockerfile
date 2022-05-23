FROM envoyproxy/envoy:v1.21-latest

COPY mockaroo.yaml /etc/envoy/mockaroo.yaml
CMD /usr/local/bin/envoy -c /etc/envoy/mockaroo.yaml