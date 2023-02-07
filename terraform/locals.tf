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
  enable_mssql_database                           = var.enable_mssql_database
  enable_cdn_frontdoor                            = var.enable_cdn_frontdoor
  cdn_frontdoor_enable_rate_limiting              = var.cdn_frontdoor_enable_rate_limiting
  cdn_frontdoor_rate_limiting_duration_in_minutes = var.cdn_frontdoor_rate_limiting_duration_in_minutes
  cdn_frontdoor_rate_limiting_threshold           = var.cdn_frontdoor_rate_limiting_threshold
  cdn_frontdoor_host_add_response_headers         = var.cdn_frontdoor_host_add_response_headers
  key_vault_access_users                          = toset(var.key_vault_access_users)
  tfvars_filename                                 = var.tfvars_filename
  enable_monitoring                               = var.enable_monitoring
  monitor_email_receivers                         = var.monitor_email_receivers
}
