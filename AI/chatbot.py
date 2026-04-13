#=================LOADING LIBRARIES===============================
import os
from dotenv import load_dotenv
import weaviate
from weaviate.classes.init import Auth
from langchain_weaviate import WeaviateVectorStore
from langchain_huggingface import HuggingFaceEmbeddings
from langchain_core.tools import create_retriever_tool
from langchain_mistralai import ChatMistralAI
from langchain.agents import create_agent
#=================LOADING ENVIRONMENT VARIABLES===============================
load_dotenv()

WEAVIATE_URL=os.getenv("WEAVIATE_URL")
WEAVIATE_API_KEY = os.getenv("WEAVIATE_API_KEY")
MISTRAL_API_KEY = os.getenv("MISTRALAI_API_KEY")

# ================== LOADING EMBEDDING MODLE  ==================
embedding = HuggingFaceEmbeddings(model_name="paraphrase-multilingual-MiniLM-L12-v2")

# ================== CONNECT TO VECTOR DATABASE  ==================

client = weaviate.connect_to_weaviate_cloud(
        cluster_url=WEAVIATE_URL,
        auth_credentials=Auth.api_key(WEAVIATE_API_KEY),
    )


vector_store = WeaviateVectorStore(
        client=client,
        index_name="Down_Syndrome" ,
        text_key="text" ,
        embedding=embedding)  # 🔥 مهم جدًا (مش from_documents))

retriever = vector_store.as_retriever(search_kwargs={"k": 3})

retriever_tool = create_retriever_tool(
        retriever,
        "Down_Syndrome_retriever",
        "Search information about Down Syndrome"
    )
# ================== CREATE MODEL ==================

model = ChatMistralAI(
        model="mistral-large-latest",
        api_key=MISTRAL_API_KEY)
# ================== CREATE AGENT ==================

agent = create_agent(
        model=model,
        tools=[retriever_tool],
        system_prompt="You are a helpful assistant that can answer questions about Down Syndrome."
    )


# ================== ASK ==================
def ask_question(query: str):

    response = agent.invoke({
        "messages": [{"role": "user", "content": query}]
    })

    return response["messages"][-1].content


# ================== CLOSE CONNECTION==================
def close_connection():
    client.close()
# ================== TESTING RAG APP==================

if __name__ == "__main__":
    try:
        response = ask_question("ما هى متلازمة داون")
        print(response)
    finally:
        close_connection()
