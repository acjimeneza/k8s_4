# k8s_4

## Creación de la imagen base

El primer paso es la construcción de la imagen base, para esto se deben ejecutar los siguientes comandos:

```
cd 1-ImagenBase
docker build -f .docker/Dockerfile -t nombre-usuario/ms-base:1 .
```
docker build -f .docker/Dockerfile -t nombre-usuario/ms-base:1 .
Si se desea validar la imagen local, el usuario debe ejecutar la imagen creada en docker:

```
docker run -p 5000:5000 -d nombre-usuario/ms-base:1
```

Luego, se debe abrir el naevgador y ejecutar la siguiente URL para ingresar al swagger `http://localhost:5000/swagger`

Para bublicar la imagen en nuestros dockerHub se deben ejecutar los siguientes comandos:

```
docker login # Desde la consola ingresar el nombre de usuario y la contraseña
docker push nombre-usuario/ms-base:1
```

## Manejo de Secretos y Configmap

### Configmap

Desde la raíz del repositorio, ingresar a la carpeta:

```
cd 2-Deploy-Secrets/Configmap
```

Luego se debe ejecutar los siguientes comandos, para la creación del configmap, del servicio y del despliegue:

```
kubectl apply -f configmap.yml
kubectl apply -f deployConfigMap.yml
```

Los comandos anteriores se pueden simplificar, especificando en la ejecución del comando de *apply* la carpeta. Suponiendo que nos encontramos en la carpeta `2-Deploy-Secrets`, se ejecuta el siguiente comando:

```
kubectl apply -f Configmap
```

No olvidar elminiar los recursos creados con el comando `kubectl delete ...`


### Secretos

Desde la raíz del repositorio, ingresar a la carpeta:


```
kubectl apply -f Secrets
```

No olvidar elminiar los recursos creados con el comando `kubectl delete -f Secrets`


Tips para convertir valores de texto plano a base 64, en una terminal se debe ejecutar el siguiente comando:

```
echo -n 'valor a convertir' | base64
```

## Servicio

Para mayor información de los tipos de servicios, se recomienda la información suminstrada por (Kubernetes)[https://kubernetes.io/docs/concepts/services-networking/service/]

## Volumenes

Para inciar con el manejo de persistencia usando volumenes, es interesante iniciar con una imagen que necesite de persistencia para el almacenamiento de la información. Para este caso, se hace uso de una imagen de MySQL.

Para crear esta imagen, es necesario estar en la carpeta `1-ImagenBase` y ejecutar los siguientes comandos:

```
docker build -f .docker/DockerfileMySql -t acjimeneza/mysql:1 .
docker push acjimeneza/mysql:1
```

En caso de querer validar la imagen de MySQL de forma local, se recomienda ejecutar el siguiente comando:

```
docker run -it --rm -p 3306:3306 --name mysql -e MYSQL_ROOT_PASSWORD=1234 -e MYSQL_USER=admin -e MYSQL_PASSWORD=1234 -e MYSQL_DATABASE=Timedb acjimeneza/mysql:1
```

Luego de validar y publicar la imagen en el repositorio (en este caso Docker Hub), se debe ir a la carpeta `5-Ingress` y desde allí ejectutar:

```
kubectl apply -f config.yml
```

Lo anterior permite crear las configuración para las conexiones y manjeo de las bases de datos. Para poder crear los volumenes, es importante reconecer que tipo de *storageClassName* se pueden crear en nuestro cluster. Afortunadamente se puede saber ejecutando el siguiente comando:

```
kubectl get storageclass 
```

Validando el tipo de recurso que podemos usar en *storageClassName* ejecutamos los siguientes comandos:


```
cd Persistance
kubectl apply -f persistentVolume.yml

# Se debe esperar a que el volumen se termine de crear

kubectl apply -f persistentVolume.yml
```

Luego del despleigue puden que encuentren errores a la hora de la conexión, esto se debe a que la configuración dentro de la imagen se sobre pone al hacer el montaje del volumen, para lo cual, se deben ejecutar los siguientes comandos:

```
kubectl exec -it pod/mysql-deploy-0 -- bin/bash

#root> mysql -u root -p
password:<vacio>

mysql>
```

Ya dentro de MySQL, se debe ejecutar el siguiente script, el cual puede variar dependiendo si se realizaron cambos en las configuracionesL

```sql
CREATE DATABASE Timedb;

CREATE USER 'admin'@'%' IDENTIFIED BY '1234';

CREATE TABLE Timedb.TimeData (
id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
Date DATETIME NOT NULL,
Number INT  NOT NULL);

GRANT ALL PRIVILEGES ON * . * TO 'admin'@'%';
```

El anterior script se encuentra en `./4-Volumenes/Persistance/initSql.sql`

## Ingress

Para habilitar el ingress de nginx en docker desktop se debe usar el siguiente comando:

```
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v0.46.0/deploy/static/provider/cloud/deploy.yaml
```

Para validar la instalación, se puede ver el estado de los pods en el namespace de *ingress-nginx*, y además ver que los custome resource definitions, se encuentran actualizados

```
kubectl api-versions
kubectl api-resources
```

Luego de validar el estado de instalación, se deben ejecutar los siguientes comandos:

```
kubectl apply -f deployService1.yml
kubectl apply -f deployService2-3.yml
kubectl apply -f ingress.yml
```
