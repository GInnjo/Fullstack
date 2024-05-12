terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
  }
}

provider "azurerm" {
  features {}
}

#-------------------------------------   NETWORKS    --------------------------------------#

resource "azurerm_resource_group" "fullstack-rg" {
  name     = "hamk-full-stack"
  location = "Sweden Central"
}

resource "azurerm_virtual_network" "fullstack-vn" {
  name                = "fullstack-network"
  resource_group_name = azurerm_resource_group.fullstack-rg.name
  location            = azurerm_resource_group.fullstack-rg.location
  address_space       = ["10.0.0.0/16"]
}

resource "azurerm_subnet" "fullstack-subn" {
  name                 = "fullstack-subnet"
  virtual_network_name = azurerm_virtual_network.fullstack-vn.name
  address_prefixes     = ["10.0.1.0/24"]
  resource_group_name  = azurerm_resource_group.fullstack-rg.name
}

#-------------------------------------   VM ADDRESSES   --------------------------------------#

resource "azurerm_public_ip" "fullstack-public-ip" {
  name                = "fullstack-public-ip"
  location            = azurerm_resource_group.fullstack-rg.location
  resource_group_name = azurerm_resource_group.fullstack-rg.name
  allocation_method   = "Dynamic"
}

resource "azurerm_public_ip" "fullstack-client-public-ip" {
  name                = "fullstack-client-public-ip"
  location            = azurerm_resource_group.fullstack-rg.location
  resource_group_name = azurerm_resource_group.fullstack-rg.name
  allocation_method   = "Dynamic"
}

resource "azurerm_network_interface" "fullstack-inf" {
  name                = "fullstack-inf"
  location            = azurerm_resource_group.fullstack-rg.location
  resource_group_name = azurerm_resource_group.fullstack-rg.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.fullstack-subn.id
    private_ip_address_allocation = "Static"
    private_ip_address            = "10.0.1.10"
    public_ip_address_id          = azurerm_public_ip.fullstack-public-ip.id
  }
}

resource "azurerm_network_interface" "fullstack-client-inf" {
  name                = "fullstack-client-inf"
  location            = azurerm_resource_group.fullstack-rg.location
  resource_group_name = azurerm_resource_group.fullstack-rg.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.fullstack-subn.id
    private_ip_address_allocation = "Static"
    private_ip_address            = "10.0.1.100"
    public_ip_address_id          = azurerm_public_ip.fullstack-client-public-ip.id
  }
}

#-------------------------------------------------------------------------------------------#
#-------------------------------------   SERVER VM    --------------------------------------#
#-------------------------------------------------------------------------------------------#
#VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV#

resource "azurerm_linux_virtual_machine" "fullstack-server" {

  depends_on = [
    azurerm_network_interface.fullstack-inf
  ]

  name                = "fullstack-webserver"
  resource_group_name = azurerm_resource_group.fullstack-rg.name
  location            = azurerm_resource_group.fullstack-rg.location
  size                = "Standard_B1s"

  network_interface_ids = [azurerm_network_interface.fullstack-inf.id]

  admin_username = "adminuser"

  admin_ssh_key {
    username   = "adminuser"
    public_key = file(".ssh/id_rsa.pub")
  }

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "Canonical"
    offer     = "0001-com-ubuntu-server-focal"
    sku       = "20_04-lts"
    version   = "latest"
  }

}

#-------------------------------------------------------------------------------------------#
#-------------------------------------   CLIENT VM    --------------------------------------#
#-------------------------------------------------------------------------------------------#
#VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV#

resource "azurerm_linux_virtual_machine" "fullstack-job-client" {

  depends_on = [
    azurerm_network_interface.fullstack-inf
  ]

  name                = "fullstack-job-client"
  resource_group_name = azurerm_resource_group.fullstack-rg.name
  location            = azurerm_resource_group.fullstack-rg.location
  size                = "Standard_B1s"

  network_interface_ids = [azurerm_network_interface.fullstack-client-inf.id]

  admin_username = "adminuser"

  admin_ssh_key {
    username   = "adminuser"
    public_key = file(".ssh/id_rsa.pub")
  }

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "Canonical"
    offer     = "0001-com-ubuntu-server-focal"
    sku       = "20_04-lts"
    version   = "latest"
  }



}

#---- wAIT 10 seconds----#

resource "time_sleep" "seconds_10" {
  depends_on = [
    azurerm_linux_virtual_machine.fullstack-server,
    azurerm_linux_virtual_machine.fullstack-job-client,
    azurerm_public_ip.fullstack-public-ip
    ]

  create_duration = "10s"
}

data "azurerm_public_ip" "server" {
  depends_on = [time_sleep.seconds_10]

  name       = azurerm_public_ip.fullstack-public-ip.name
  resource_group_name = azurerm_resource_group.fullstack-rg.name
}

data "azurerm_public_ip" "client" {
  depends_on = [time_sleep.seconds_10]

  name       = azurerm_public_ip.fullstack-client-public-ip.name
  resource_group_name = azurerm_resource_group.fullstack-rg.name
}

resource "time_sleep" "seconds_5" {
  depends_on = [time_sleep.seconds_10]

  create_duration = "5s"
}



#-------------------------------------------------------------------------------------------#
#------------------------------------- SERVER SCRIPTS --------------------------------------#
#-------------------------------------------------------------------------------------------#
#VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV#

resource "null_resource" "provisioning_server" {
  depends_on = [time_sleep.seconds_5]

      connection {
      type        = "ssh"
      host        = data.azurerm_public_ip.server.ip_address
      user        = "adminuser"
      private_key = file(".ssh/id_rsa")
    }

  provisioner "remote-exec" {

    inline = [
      "sudo apt-get update -y",
      "sudo apt-get install -y docker.io",
      "sudo apt-get install -y apt-transport-https ca-certificates curl software-properties-common",
      "curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -",
      "sudo add-apt-repository \"deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable\"",
      "sudo apt-get install -y docker-ce",
      "sudo usermod -aG docker $USER",
      "sudo systemctl enable docker",
      "sudo systemctl start docker",
      "curl -fsSL https://apt.releases.hashicorp.com/gpg | sudo apt-key add -",
      "sudo apt-add-repository \"deb [arch=amd64] https://apt.releases.hashicorp.com $(lsb_release -cs) main\"",
      "docker login ghcr.io -u ginnjo -p TOKEN",
      "curl -o docker-compose.yaml https://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@raw.githubusercontent.com/GInnjo/Fullstack/master/docker-compose.yaml",
      "curl -o Caddyfile https://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@raw.githubusercontent.com/GInnjo/Fullstack/master/Caddyfile",
    ]
  }
}

#-------------------------------------------------------------------------------------------#
#------------------------------------- CLIENT SCRIPTS --------------------------------------#
#-------------------------------------------------------------------------------------------#
#VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV#

resource "null_resource" "provisioning_client" {
  depends_on = [time_sleep.seconds_5]

      connection {
      type        = "ssh"
      host        = data.azurerm_public_ip.client.ip_address
      user        = "adminuser"
      private_key = file(".ssh/id_rsa")
    }

  provisioner "remote-exec" {


    inline = [
      "sudo apt-get update -y",
      "sudo apt-get install -y docker.io",
      "sudo apt-get install -y apt-transport-https ca-certificates curl software-properties-common",
      "curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -",
      "sudo add-apt-repository \"deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable\"",
      "sudo apt-get update -y",
      "sudo apt-get install -y docker-ce",
      "sudo usermod -aG docker $USER",
      "sudo systemctl enable docker",
      "sudo systemctl start docker",
      "curl -fsSL https://apt.releases.hashicorp.com/gpg | sudo apt-key add -",
      "sudo apt-add-repository \"deb [arch=amd64] https://apt.releases.hashicorp.com $(lsb_release -cs) main\"",
      "sudo apt-get update -y",
      "sudo apt-get install -y nomad",
      "sudo touch /etc/nomad.d/client.hcl",
      "sudo bash -c 'cat << EOF > /etc/nomad.d/client.hcl",
      "data_dir = \"/node-data\"",
      " ",
      "client {",
      "  enabled = true",
      "  server_join {",
      "    retry_join = [\"${azurerm_network_interface.fullstack-inf.private_ip_address}:4647\"]",
      "  }",
      "}",
      " ",
      "EOF'",
      "cat /etc/nomad.d/client.hcl",
      "sudo mkdir -p /node-data/plugins",
      "sudo systemctl enable nomad",
    ]
  }
}

resource "null_resource" "provisioning_server_up" {
  depends_on = [
    null_resource.provisioning_server,
    null_resource.provisioning_client
  ]

  provisioner "remote-exec" {
    inline = [
      "cd ~/",
      "docker compose up > /dev/null 2>&1 &"
    ]

    connection {
      type        = "ssh"
      host        = data.azurerm_public_ip.server.ip_address
      user        = "adminuser"
      private_key = file(".ssh/id_rsa")
    }
  }
}

resource "null_resource" "provisioning_client_up" {
  depends_on = [null_resource.provisioning_server_up]

      connection {
      type        = "ssh"
      host        = data.azurerm_public_ip.client.ip_address
      user        = "adminuser"
      private_key = file(".ssh/id_rsa")
    }

  provisioner "remote-exec" {


    inline = [
      "cd ~/",
      "sudo nomad agent -config=/etc/nomad.d/client.hcl > /dev/null 2>&1 &",
    ]
  }
}
