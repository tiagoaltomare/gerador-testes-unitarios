# GeradorTestesUnitarios
Esse projeto surgiu da necessidade de melhorar a cobertura de testes unitários em projetos legados dentro da squad onde atuo. Em uma solution .net tínhamos muitos contratos herdados da alta plataforma com diversas propriedades, tendo assim que cobrir os métodos de get e set delas para garantir a nota minima de cobertura esperada no cliente. O trabalho para fazer esses testes é muito manual e extremamente repetitivo, sendo assim surgiu como uma oportunidade a criação de algo para automatizar a geração desses métodos, com a possibilidade de gerar ainda uma estrutura para os métodos de negócio.

# Execução
1. Criar uma pasta chamada "Files" no C: do computador
2. Dentro dela crie outras duas pastas "In" e "Out". Dentro da pasta In coloque as dlls para as quais deseja gerar as classes de teste.
3. Execute a aplicação, confira o log do console, para eventuais erros. Se correr tudo bem, os resultados serão gerados dentro da pasta Out.

# Configurações
O arquivo Configuracoes.cs, contém as constantes com os códigos (textos) base para geração das classes de teste, incluindo as annotations que decoram as classes, por exemplo [TestMethod]. Qualquer alteração necessária para adequação ao contexto de outro projeto deve ser realizada nesse arquivo.