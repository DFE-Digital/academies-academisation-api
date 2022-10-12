variable "environment" {
  description = "Environment name. Will be used along with `project_name` as a prefix for all resources."
  type        = string
}

variable "tfvars_filename" {
  description = "tfvars filename. This ensures that tfvars are kept up to date in Key Vault."
  type        = string
}

variable "project_name" {
  description = "Project name. Will be used along with `environment` as a prefix for all resources."
  type        = string
}

variable "azure_location" {
  description = "Azure location in which to launch resources."
  type        = string
}

variable "tags" {
  description = "Tags to be applied to all resources"
  type        = map(string)
}

variable "virtual_network_address_space" {
  description = "Virtual network address space CIDR"
  type        = string
}

variable "enable_container_registry" {
  description = "Set to true to create a container registry"
  type        = bool
}

variable "image_name" {
  description = "Image name"
  type        = string
}

variable "container_command" {
  description = "Container command"
  type        = list(any)
}

variable "container_secret_environment_variables" {
  description = "Container secret environment variables"
  type        = map(string)
  sensitive   = true
}

variable "enable_mssql_database" {
  description = "Set to true to create an Azure SQL server/database, with aprivate endpoint within the virtual network"
  type        = bool
}
