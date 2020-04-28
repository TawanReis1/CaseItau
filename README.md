# API Informações Campeonato

API possui como objetivo retornar dados estatísticos do campeonato brasileiro, entre o período de 2015 a 2019.

## Instalação

Baixe o projeto através do GitHub ou copie o comando e execute em seu console.

É imprescindível que você tenha o git instalado em sua máquina para executar o comando acima, caso não tenha, pode realizar o download [aqui](https://git-scm.com/downloads).

```bash
git clone https://github.com/TawanReis1/CaseItau.git
```

Após o download ser concluído, execute o projeto com o auxílio do Visual Studio, caso não tenha em sua máquina, pode realizar o download [aqui](https://visualstudio.microsoft.com/pt-br/downloads/).


## Utilização
***OBS: Os dados são recuperados a partir do arquivo .txt que está no caminho seguinte caminho: '../CaseItau/DadosCampeonato.txt. Caso esse arquivo tenha o seu local alterado, o funcionamento da API será comprometido.'***

A utilização da API é realizada através de um Swagger, onde é possível realizar as chamadas de uma maneira um pouco mais intuitiva. Essa é a visão incial:

![Swagger reduzido](https://user-images.githubusercontent.com/40872077/80543622-aa5b7e80-8985-11ea-999c-8aa7432f29e9.png)


### /informacoes-campeonato/time
Esse recurso requere que seja informado o nome de um time, para que ele realize uma busca nas tabelas de classificação do campeonato brasileiro, entre o período de 2015 a 2019, e traga informações relevantes, como por exemplo a posição dele levando em consideração as cinco edições do campeonato, a pontuação total que ele conquistou nesse período, a quantidade total de vitórias, etc. 

O nome do time não precisa ter acentuação ou que as iniciais sejam maiúsculas, a API possui a capacidade de não levar essas características em consideração.

![GetByTime](https://user-images.githubusercontent.com/40872077/80544255-070b6900-8987-11ea-9b9a-a95f931b5236.png)

### /informacoes-campeonato/estado
Esse recurso é bem similar ao anterior, o que difere que agora é necessário inserir a sigla do estado que é desejado realizar a busca. A API ira realizar a soma de todos os dados dos times pertencentes àquele estado, e irá retornar informações relevantes.

![getByState](https://user-images.githubusercontent.com/40872077/80544838-471f1b80-8988-11ea-9d2c-5e6caada97af.png)


### /informacoes-campeonato/complementar
E por fim mas não menos importante, existe esse recurso. Ele irá trazer dados que exibem os melhores e piores times do campeonato nesse período, em diferentes quesitos.

Não existe a necessidade de  passar nenhum valor adicional neste recurso.

![getComplementar](https://user-images.githubusercontent.com/40872077/80545220-3622da00-8989-11ea-8fd2-92b7e9db6cf4.png)
