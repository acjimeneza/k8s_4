# k8s_4

## Creación de la imagen base

El primer paso es la construcción de la imagen base, para esto se deben ejecutar los siguientes comandos:

```
cd 1-ImagenBase
docker build -f .docker/Dockerfile -t nombre-usuario/ms-base:1 .
```

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

## Volumenes

docker build -f .docker/DockerfileMySql -t acjimeneza/mysql:1 .
docker push acjimeneza/mysql:1

docker run -it --rm -p 3306:3306 --name mysql -e MYSQL_ROOT_PASSWORD=1234 -e MYSQL_USER=admin -e MYSQL_PASSWORD=1234 -e MYSQL_DATABASE=Timedb acjimeneza/mysql:1
