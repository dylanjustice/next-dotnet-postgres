FROM envoyproxy/envoy:v1.21-latest
COPY front-envoy.yaml /etc/envoy/front-envoy.yaml
CMD /usr/local/bin/envoy -c /etc/envoy/front-envoy.yaml