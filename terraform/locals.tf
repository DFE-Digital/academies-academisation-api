locals {
  environment                                     = var.environment
  project_name                                    = var.project_name
  azure_location                                  = var.azure_location
  tags                                            = var.tags
  virtual_network_address_space                   = var.virtual_network_address_space
  enable_container_registry                       = var.enable_container_registry
  image_name                                      = var.image_name
  container_command                               = var.container_command
  container_secret_environment_variables          = var.container_secret_environment_variables
  enable_event_hub                                = var.enable_event_hub
  enable_mssql_database                           = var.enable_mssql_database
  enable_dns_zone                                 = var.enable_dns_zone
  dns_zone_domain_name                            = var.dns_zone_domain_name
  dns_ns_records                                  = var.dns_ns_records
  dns_txt_records                                 = var.dns_txt_records
  enable_cdn_frontdoor                            = var.enable_cdn_frontdoor
  cdn_frontdoor_enable_rate_limiting              = var.cdn_frontdoor_enable_rate_limiting
  cdn_frontdoor_rate_limiting_duration_in_minutes = var.cdn_frontdoor_rate_limiting_duration_in_minutes
  cdn_frontdoor_rate_limiting_threshold           = var.cdn_frontdoor_rate_limiting_threshold
  cdn_frontdoor_host_add_response_headers         = var.cdn_frontdoor_host_add_response_headers
  cdn_frontdoor_custom_domains                    = var.cdn_frontdoor_custom_domains
  cdn_frontdoor_origin_fqdn_override              = var.cdn_frontdoor_origin_fqdn_override
  cdn_frontdoor_origin_host_header_override       = var.cdn_frontdoor_origin_host_header_override
  key_vault_access_users                          = toset(var.key_vault_access_users)
  key_vault_access_ipv4                           = var.key_vault_access_ipv4
  tfvars_filename                                 = var.tfvars_filename
  enable_monitoring                               = var.enable_monitoring
  monitor_email_receivers                         = var.monitor_email_receivers
  container_health_probe_path                     = var.container_health_probe_path
  cdn_frontdoor_health_probe_path                 = var.cdn_frontdoor_health_probe_path
  monitor_endpoint_healthcheck                    = var.monitor_endpoint_healthcheck
  monitor_enable_slack_webhook                    = var.monitor_enable_slack_webhook
  monitor_slack_webhook_receiver                  = var.monitor_slack_webhook_receiver
  monitor_slack_channel                           = var.monitor_slack_channel
  existing_network_watcher_name                   = var.existing_network_watcher_name
  existing_network_watcher_resource_group_name    = var.existing_network_watcher_resource_group_name
}
