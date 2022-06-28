from inspect import _void
from lib2to3.pytree import convert
import re
from traceback import print_tb
import statistics


idade = []
resposta = []
n = 0 

n = int(input("Insira quantas pessoas foram entrevistadas:"))

print(n ,"pessoas foram entrevistadas")


for count in range(n):
    print("Nova avaliação")
    idadeInserida = int(input("Insira a idade do avaliado:"))
    respostaInserida = int(input("Insira a resposta do avaliado:"))
    idade.insert(count, idadeInserida)
    resposta.insert(count, respostaInserida)
    count+=1

print(idade, resposta)

mediaresposta= sum(resposta)/len(resposta)
print ("resposta média é:",mediaresposta)

#mediaresposta30 = sum(resposta>30)/len(resposta>30)
#print("resposta média de menores de 30 anos", mediaresposta30)

resposta.sort
print(resposta)


