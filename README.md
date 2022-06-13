# WiproDesafioBackend
Desenvolvedor - Carlos Alexandre Cavalheiro

Para executar a aplicação:
1) Executar o script de criação do banco de dados em SQL Server: 'ScriptWiproDesafioBD.sql'
2) Abrir o Projeto Desafio1 (Referente a API de comunicação) no Visual Studio!
3) Executar o projeto em como uma instância em segundo plano sem depuração 'Ctrl + F5' ou 'Menu Depurar\Executar sem depuração'
4) Selecione o endpoint 'AddItemFila' e clique em Tryout. 
5) Inserir os dados de entrada do arquivo fila_moeda.json ou abaixo e executar:
```
[
 {
 "moeda": "USD",
 "data_inicio": "2010-01-01",
 "data_fim": "2010-12-01"
 },
 {
 "moeda": "EUR",
 "data_inicio": "2010-12-01",
 "data_fim": "2020-01-01"
 },
 {
 "moeda": "JPY",
 "data_inicio": "2000-03-11",
 "data_fim": "2000-03-30"
 }
]
```
6) Abrir o Projeto Desafio2 (Referente a importação dos arquivos 'DadosMoeda.csv' e 'DadosCotacao.csv' e geração de arquivo de Resultado)
7) Editar a ConnectString corresponde as configuraçõs do SQL Server: 'Data\DBConnect.cs'
8) Executar o Desafio2;
9) Autmáticamente a cada 2 minutos o aplicação executa uma chamada no endpoint 'GetItemFila' em busca da última moeda adicionada na fila que será removida da fila e gerado arquivo de Resultado.

