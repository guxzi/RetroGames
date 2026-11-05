-- =============================================================================
-- RetroGames · Script 04 · Testes de integridade (Aula 1, etapa 5)
--
-- Como rodar no Workbench: execute UM comando por vez (cursor na linha +
-- Ctrl+Enter). Os quatro primeiros DEVEM dar erro; o Workbench para o script
-- inteiro no primeiro erro se você usar "Execute All".
--
-- O "Resultado obtido" de cada teste foi registrado num MySQL 8.0 com os
-- dados do script 02. Confira no seu Workbench e corrija o comentário se a
-- mensagem for diferente.
-- =============================================================================

USE retrogames;

-- -----------------------------------------------------------------------------
-- Teste 1: inserir jogo com co_plataforma inexistente
-- Esperado: erro 1452 (chave estrangeira)
-- -----------------------------------------------------------------------------
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                     dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('TESTE INTEGRIDADE', 1990, 50.00, 1, UTC_TIMESTAMP(), 9999, 2);
-- Resultado obtido: Error Code 1452. Cannot add or update a child row:
--   a foreign key constraint fails (fk_jogo_plataforma)

-- -----------------------------------------------------------------------------
-- Teste 2: inserir plataforma com nome já cadastrado
-- Esperado: erro 1062 (UNIQUE)
-- -----------------------------------------------------------------------------
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento)
VALUES ('Super Nintendo', 'Nintendo', 1990);
-- Resultado obtido: Error Code 1062. Duplicate entry 'Super Nintendo'
--   for key 'tb_plataforma.uk_plataforma_no_plataforma'

-- -----------------------------------------------------------------------------
-- Teste 3: excluir plataforma que possui jogos
-- Esperado: erro 1451 (chave estrangeira)
-- -----------------------------------------------------------------------------
DELETE FROM tb_plataforma
WHERE no_plataforma = 'Super Nintendo';
-- Resultado obtido: Error Code 1451. Cannot delete or update a parent row:
--   a foreign key constraint fails (fk_jogo_plataforma)
-- Obs.: se o Workbench reclamar de "safe update mode" (erro 1175), rode
--   SET SQL_SAFE_UPDATES = 0; antes. O erro esperado continua sendo o 1451.

-- -----------------------------------------------------------------------------
-- Teste 4: inserir jogo com no_jogo nulo
-- Esperado: erro 1048 (NOT NULL)
-- -----------------------------------------------------------------------------
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                     dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES (NULL, 1990, 50.00, 1, UTC_TIMESTAMP(),
        (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'NES'), 2);
-- Resultado obtido: Error Code 1048. Column 'no_jogo' cannot be null

-- -----------------------------------------------------------------------------
-- Teste 5: inserir jogo com vl_preco igual a 0
-- Esperado: ACEITO. O banco não tem CHECK; a regra de preço > 0 fica no
-- Service (RN06).
-- -----------------------------------------------------------------------------
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                     dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('TESTE INTEGRIDADE', 1990, 0.00, 1, UTC_TIMESTAMP(),
        (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'NES'), 2);
-- Resultado obtido: 1 row(s) affected (aceito)

-- -----------------------------------------------------------------------------
-- Limpeza: remove o que os testes inseriram
-- -----------------------------------------------------------------------------
DELETE FROM tb_jogo
WHERE no_jogo = 'TESTE INTEGRIDADE';

-- Conferência: esperado 0 jogos de teste e 25 jogos no total
SELECT
    (SELECT COUNT(*) FROM tb_jogo WHERE no_jogo = 'TESTE INTEGRIDADE') AS jogos_de_teste,
    (SELECT COUNT(*) FROM tb_jogo) AS total_jogos;
