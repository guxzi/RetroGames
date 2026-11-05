-- =============================================================================
-- RetroGames · Script 01 · Criação do banco e das tabelas
-- Referência: Espec. 4.1 (nomes), 4.2 (dicionário de dados) e 4.3 (restrições)
--
-- Regras seguidas:
--   * O banco guarda só a estrutura: PK, FK (comportamento padrão), tipos,
--     NOT NULL e UNIQUE. Nada de CHECK, trigger, function ou ON DELETE CASCADE.
--   * Somente ds_jogo aceita NULL.
--   * Tabelas de domínio (tb_condicao_jogo, tb_perfil) usam chave fixa,
--     sem AUTO_INCREMENT.
--
-- Para recriar do zero: DROP DATABASE retrogames; e rode este script de novo.
-- =============================================================================

CREATE DATABASE retrogames
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_0900_ai_ci;

USE retrogames;

-- -----------------------------------------------------------------------------
-- Domínio: condição de conservação do jogo (1 Novo, 2 Usado, 3 Recondicionado)
-- -----------------------------------------------------------------------------
CREATE TABLE tb_condicao_jogo (
    co_condicao_jogo INT         NOT NULL,
    ds_condicao_jogo VARCHAR(50) NOT NULL,
    CONSTRAINT pk_condicao_jogo PRIMARY KEY (co_condicao_jogo),
    CONSTRAINT uk_condicao_jogo_ds_condicao_jogo UNIQUE (ds_condicao_jogo)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Domínio: perfis de acesso (1 Administrador, 2 Cliente)
-- -----------------------------------------------------------------------------
CREATE TABLE tb_perfil (
    co_perfil INT         NOT NULL,
    ds_perfil VARCHAR(50) NOT NULL,
    CONSTRAINT pk_perfil PRIMARY KEY (co_perfil),
    CONSTRAINT uk_perfil_ds_perfil UNIQUE (ds_perfil)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Cadastro: console ou plataforma
-- -----------------------------------------------------------------------------
CREATE TABLE tb_plataforma (
    co_seq_plataforma INT          NOT NULL AUTO_INCREMENT,
    no_plataforma     VARCHAR(100) NOT NULL,
    no_fabricante     VARCHAR(100) NOT NULL,
    nu_ano_lancamento INT          NOT NULL,
    CONSTRAINT pk_plataforma PRIMARY KEY (co_seq_plataforma),
    CONSTRAINT uk_plataforma_no_plataforma UNIQUE (no_plataforma)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Cadastro (somente leitura no sistema): gênero do jogo
-- -----------------------------------------------------------------------------
CREATE TABLE tb_genero (
    co_seq_genero INT         NOT NULL AUTO_INCREMENT,
    no_genero     VARCHAR(50) NOT NULL,
    CONSTRAINT pk_genero PRIMARY KEY (co_seq_genero),
    CONSTRAINT uk_genero_no_genero UNIQUE (no_genero)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Principal: jogo à venda
-- -----------------------------------------------------------------------------
CREATE TABLE tb_jogo (
    co_seq_jogo       INT           NOT NULL AUTO_INCREMENT,
    no_jogo           VARCHAR(150)  NOT NULL,
    ds_jogo           VARCHAR(1000) NULL,
    nu_ano_lancamento INT           NOT NULL,
    vl_preco          DECIMAL(10,2) NOT NULL,
    qt_estoque        INT           NOT NULL,
    st_ativo          BOOLEAN       NOT NULL DEFAULT TRUE,
    dt_cadastro       DATETIME      NOT NULL,
    co_plataforma     INT           NOT NULL,
    co_condicao_jogo  INT           NOT NULL,
    CONSTRAINT pk_jogo PRIMARY KEY (co_seq_jogo),
    CONSTRAINT fk_jogo_plataforma
        FOREIGN KEY (co_plataforma) REFERENCES tb_plataforma (co_seq_plataforma),
    CONSTRAINT fk_jogo_condicao_jogo
        FOREIGN KEY (co_condicao_jogo) REFERENCES tb_condicao_jogo (co_condicao_jogo),
    INDEX ix_jogo_no_jogo (no_jogo)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Associativa: jogo N:N gênero (chave primária composta)
-- -----------------------------------------------------------------------------
CREATE TABLE tb_jogo_genero (
    co_jogo   INT NOT NULL,
    co_genero INT NOT NULL,
    CONSTRAINT pk_jogo_genero PRIMARY KEY (co_jogo, co_genero),
    CONSTRAINT fk_jogo_genero_jogo
        FOREIGN KEY (co_jogo) REFERENCES tb_jogo (co_seq_jogo),
    CONSTRAINT fk_jogo_genero_genero
        FOREIGN KEY (co_genero) REFERENCES tb_genero (co_seq_genero)
) ENGINE = InnoDB;

-- -----------------------------------------------------------------------------
-- Cadastro: usuário do sistema (senha guardada só como hash)
-- -----------------------------------------------------------------------------
CREATE TABLE tb_usuario (
    co_seq_usuario INT          NOT NULL AUTO_INCREMENT,
    no_usuario     VARCHAR(100) NOT NULL,
    ds_email       VARCHAR(150) NOT NULL,
    ds_senha_hash  VARCHAR(255) NOT NULL,
    st_ativo       BOOLEAN      NOT NULL DEFAULT TRUE,
    dt_cadastro    DATETIME     NOT NULL,
    co_perfil      INT          NOT NULL,
    CONSTRAINT pk_usuario PRIMARY KEY (co_seq_usuario),
    CONSTRAINT uk_usuario_ds_email UNIQUE (ds_email),
    CONSTRAINT fk_usuario_perfil
        FOREIGN KEY (co_perfil) REFERENCES tb_perfil (co_perfil)
) ENGINE = InnoDB;
