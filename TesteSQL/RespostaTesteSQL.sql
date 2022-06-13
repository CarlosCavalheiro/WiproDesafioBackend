/**--Exercicio 1 - TESTE SQL--**/
SELECT count(*) as TotalProcessos
	   ,sum(case when s.idStatus = 1 then 1 else 0 end) as TotalAberto 
	   ,sum(case when s.idStatus = 2 then 1 else 0 end) as TotalEncerrado 
	   FROM tb_Processo p 
	   INNER JOIN tb_Status s ON s.idStatus = p.idStatus;

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
		Where p.idStatus = 2
			And YEAR(p.DtEncerramento) = '2013'
	    Order By a.dtAndamento DESC;

/**--Exercicio 3 - TESTE SQL--**/
SELECT p.DtEncerramento,
		Count(*) as Qtd
		From tb_Processo p	   	   
		Group by p.DtEncerramento
		HAVING Count(*) > 5
		
/**--Exercicio 4 - TESTE SQL--**/
SELECT REPLICATE('0',12-LEN(p.nroProcesso)) + RTRIM(p.nroProcesso) as nroProcesso
	   FROM tb_Processo p
	   
