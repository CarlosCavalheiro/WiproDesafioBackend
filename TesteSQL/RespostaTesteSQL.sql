
1) Com base no modelo acima, escreva um comando SQL que liste a quantidade de processos por 
Status com sua descrição.
/**--Exercicio 1 - TESTE SQL--**/
SELECT idStatus,
	dsStatus,
	(select count(1) from tb_Processo p Where s.idStatus = p.idStatus) as TotalProcessos
FROM tb_Status s

/* Outra Forma*/
SELECT count(*) as TotalProcessos
	   ,sum(case when s.idStatus = 1 then 1 else 0 end) as TotalAberto 
	   ,sum(case when s.idStatus = 2 then 1 else 0 end) as TotalEncerrado 
	   FROM tb_Processo p 
	   INNER JOIN tb_Status s ON s.idStatus = p.idStatus;

2) Com base no modelo acima, construa um comando SQL que liste a maior data de andamento 
por número de processo, com processos encerrados no ano de 2013.
/**--Exercicio 2 - TESTE SQL--**/
SELECT a.idAndamento
	   ,a.idProcesso
	   ,p.nroProcesso
	   ,a.dtAndamento	   
	   ,p.idStatus
	   FROM tb_Andamento a 
		   INNER JOIN tb_Processo p 
		   On a.idProcesso = p.IdProcesso
		   INNER JOIN tb_Status s 
		   On p.idStatus = s.idStatus
	    Where p.idStatus = 2 /*Status encerrado*/
		   And YEAR(p.DtEncerramento) = '2013'
	    Order By a.dtAndamento DESC;

3) Com base no modelo acima, construa um comando SQL que liste a quantidade de Data de 
Encerramento agrupada por ela mesma onde a quantidade da contagem seja maior que 5.
/**--Exercicio 3 - TESTE SQL--**/
SELECT p.DtEncerramento,
		Count(*) as Qtd
		From tb_Processo p	   	   
		Group by p.DtEncerramento
		HAVING Count(*) > 5
		
4) Possuímos um número de identificação do processo, onde o mesmo contém 12 caracteres 
com zero à esquerda, contudo nosso modelo e dados ele é apresentado como bigint. Como 
fazer para apresenta-lo com 12 caracteres considerando os zeros a esquerda?
/**--Exercicio 4 - TESTE SQL--**/
SELECT REPLICATE('0',12-LEN(p.nroProcesso)) + RTRIM(p.nroProcesso) as nroProcesso
	   FROM tb_Processo p
	   
